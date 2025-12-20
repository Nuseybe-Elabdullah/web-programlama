-- Add ImagePath column to Services table (PostgreSQL)
-- Run this script to add image support to services

ALTER TABLE "Services"
ADD COLUMN "ImagePath" VARCHAR(500) NULL;

-- Optional: Update existing services with default images if needed
-- UPDATE "Services" SET "ImagePath" = '/images/default-service.jpg' WHERE "ImagePath" IS NULL;
