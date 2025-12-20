-- PostgreSQL Script to assign services to trainers without services
-- Database: GymManagementDB

-- First, let's see which trainers don't have services
SELECT t."Id", t."FullName", t."Specialization", g."Name" as "GymName"
FROM "Trainers" t
LEFT JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
INNER JOIN "Gyms" g ON t."GymId" = g."Id"
WHERE ts."TrainerId" IS NULL;

-- Assign services based on specialization
-- Yoga specialists
INSERT INTO "TrainerServices" ("TrainerId", "ServiceId")
SELECT DISTINCT t."Id", s."Id"
FROM "Trainers" t
CROSS JOIN "Services" s
LEFT JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
WHERE ts."TrainerId" IS NULL
  AND LOWER(t."Specialization") LIKE '%yoga%'
  AND LOWER(s."Name") LIKE '%yoga%';

-- Boxing specialists
INSERT INTO "TrainerServices" ("TrainerId", "ServiceId")
SELECT DISTINCT t."Id", s."Id"
FROM "Trainers" t
CROSS JOIN "Services" s
LEFT JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
WHERE ts."TrainerId" IS NULL
  AND LOWER(t."Specialization") LIKE '%boxing%'
  AND LOWER(s."Name") LIKE '%boks%';

-- LIFT/Strength specialists
INSERT INTO "TrainerServices" ("TrainerId", "ServiceId")
SELECT DISTINCT t."Id", s."Id"
FROM "Trainers" t
CROSS JOIN "Services" s
LEFT JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
WHERE ts."TrainerId" IS NULL
  AND LOWER(t."Specialization") LIKE '%lift%'
  AND s."Name" IN ('Kişisel Antrenman', 'Grup Fitness', 'Güç Antrenmanı');

-- For remaining trainers without services, assign general services from their gym
INSERT INTO "TrainerServices" ("TrainerId", "ServiceId")
SELECT DISTINCT t."Id", s."Id"
FROM "Trainers" t
CROSS JOIN "Services" s
LEFT JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
WHERE ts."TrainerId" IS NULL
  AND s."Name" IN ('Kişisel Antrenman', 'Grup Fitness')
  AND s."GymId" = t."GymId";

-- Verify the changes
SELECT t."FullName" as "Antrenör", s."Name" as "Hizmet", g."Name" as "Salon"
FROM "Trainers" t
INNER JOIN "TrainerServices" ts ON t."Id" = ts."TrainerId"
INNER JOIN "Services" s ON ts."ServiceId" = s."Id"
INNER JOIN "Gyms" g ON t."GymId" = g."Id"
ORDER BY t."FullName", s."Name";
