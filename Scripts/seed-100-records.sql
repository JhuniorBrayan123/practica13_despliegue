-- Lab 13 - Seed script: 100 new rows per entity (PostgreSQL, database "ticketerabd").
-- Safe to re-run: every row uses a "seed_" prefix + ON CONFLICT DO NOTHING.
-- All seeded users share the password: Test1234!   (BCrypt hash generated with pgcrypto, compatible with BCrypt.Net)

CREATE EXTENSION IF NOT EXISTS pgcrypto;

BEGIN;

-- 1) roles: 100 filler roles (Admin / Support / User built-ins are NOT touched)
INSERT INTO roles (role_id, role_name)
SELECT gen_random_uuid(), 'seed_role_' || lpad(i::text, 3, '0')
FROM generate_series(1, 100) AS i
ON CONFLICT (role_name) DO NOTHING;

-- 2) users: 100 users
INSERT INTO users (user_id, username, password_hash, email, created_at)
SELECT gen_random_uuid(),
       'seed_user_' || lpad(i::text, 3, '0'),
       crypt('Test1234!', gen_salt('bf', 10)),
       'seed_user_' || lpad(i::text, 3, '0') || '@lab13.local',
       now() - (random() * interval '90 days')
FROM generate_series(1, 100) AS i
ON CONFLICT (username) DO NOTHING;

-- 3) user_roles: 100 rows, one role per seed user (every 10th user is Support, the rest are User)
INSERT INTO user_roles (user_id, role_id, assigned_at)
SELECT u.user_id, r.role_id, now()
FROM (
    SELECT user_id, row_number() OVER (ORDER BY username) AS n
    FROM users
    WHERE username LIKE 'seed_user_%'
) u
JOIN roles r
  ON r.role_name = CASE WHEN u.n % 10 = 0 THEN 'Support' ELSE 'User' END
ON CONFLICT (user_id, role_id) DO NOTHING;

-- 4) tickets: 100 tickets spread across the seed users, mixed statuses
INSERT INTO tickets (ticket_id, user_id, title, description, status, created_at, closed_at)
SELECT gen_random_uuid(),
       u.user_id,
       'Seed ticket ' || lpad(i::text, 3, '0'),
       'Sample description for seed ticket ' || i,
       s.status,
       s.created_at,
       CASE WHEN s.status = 'cerrado' THEN s.created_at + interval '2 days' END
FROM generate_series(1, 100) AS i
JOIN (
    SELECT user_id, row_number() OVER (ORDER BY username) AS n
    FROM users
    WHERE username LIKE 'seed_user_%'
) u ON u.n = i
CROSS JOIN LATERAL (
    SELECT (ARRAY['abierto', 'en_proceso', 'cerrado'])[1 + (i % 3)] AS status,
           now() - (random() * interval '60 days')                 AS created_at
) s
WHERE NOT EXISTS (SELECT 1 FROM tickets t WHERE t.title = 'Seed ticket ' || lpad(i::text, 3, '0'));

-- 5) responses: 100 responses, each on a seed ticket and answered by a seed user (shifted by 7 so it is not the ticket owner)
INSERT INTO responses (response_id, ticket_id, responder_id, message, created_at)
SELECT gen_random_uuid(),
       t.ticket_id,
       u.user_id,
       'Seed response ' || lpad(t.n::text, 3, '0') || ': we are looking into it.',
       t.created_at + interval '1 hour'
FROM (
    SELECT ticket_id, created_at, row_number() OVER (ORDER BY title) AS n
    FROM tickets
    WHERE title LIKE 'Seed ticket %'
) t
JOIN (
    SELECT user_id, row_number() OVER (ORDER BY username) AS n
    FROM users
    WHERE username LIKE 'seed_user_%'
) u ON u.n = ((t.n + 7 - 1) % 100) + 1
WHERE NOT EXISTS (
    SELECT 1 FROM responses r
    WHERE r.ticket_id = t.ticket_id AND r.message LIKE 'Seed response %'
);

COMMIT;

-- Quick check
SELECT 'roles' AS entity, count(*) FROM roles WHERE role_name LIKE 'seed_role_%'
UNION ALL SELECT 'users',      count(*) FROM users     WHERE username LIKE 'seed_user_%'
UNION ALL SELECT 'user_roles', count(*) FROM user_roles ur JOIN users u ON u.user_id = ur.user_id WHERE u.username LIKE 'seed_user_%'
UNION ALL SELECT 'tickets',    count(*) FROM tickets   WHERE title LIKE 'Seed ticket %'
UNION ALL SELECT 'responses',  count(*) FROM responses WHERE message LIKE 'Seed response %';

-- Cleanup (run manually if you want to remove the seed data):
-- DELETE FROM responses  WHERE message LIKE 'Seed response %';
-- DELETE FROM tickets    WHERE title LIKE 'Seed ticket %';
-- DELETE FROM user_roles WHERE user_id IN (SELECT user_id FROM users WHERE username LIKE 'seed_user_%');
-- DELETE FROM users      WHERE username LIKE 'seed_user_%';
-- DELETE FROM roles      WHERE role_name LIKE 'seed_role_%';
