-- CatThreadService database dump for the current schema.
-- Import with: mysql -uroot -p < forum_cat_thread.sql

SET NAMES utf8mb4;
SET time_zone = '+00:00';
SET FOREIGN_KEY_CHECKS = 0;

CREATE DATABASE IF NOT EXISTS `forum_cat_thread`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE `forum_cat_thread`;

DROP TABLE IF EXISTS `Threads`;
DROP TABLE IF EXISTS `Categories`;
DROP TABLE IF EXISTS `__EFMigrationsHistory`;

CREATE TABLE `Categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `created_by` int NOT NULL,
  `created_date` date NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11
  DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `Categories`
  (`category_id`, `category_name`, `description`, `created_by`, `created_date`)
VALUES
  (1, 'General Discussion', 'A place for general discussions and off-topic chat.', 1, '2024-01-15'),
  (2, 'Programming', 'Discuss programming languages, frameworks, and tools.', 2, '2024-01-20'),
  (3, 'Web Development', 'Topics on HTML, CSS, JavaScript, and web technologies.', 3, '2024-02-10'),
  (4, 'Database Management', 'Share knowledge on SQL, NoSQL, and data storage.', 4, '2024-02-12'),
  (5, 'Mobile Development', 'Discuss Android, iOS, and mobile app development.', 2, '2024-03-05'),
  (6, 'DevOps', 'All about CI/CD, cloud services, and infrastructure automation.', 5, '2024-03-15'),
  (7, 'Machine Learning', 'A place for AI, ML, and data science enthusiasts.', 3, '2024-04-01'),
  (8, 'Cybersecurity', 'Discuss security best practices, news, and trends.', 6, '2024-04-20'),
  (9, 'Career Advice', 'Share and seek advice on careers in tech.', 1, '2024-05-10'),
  (10, 'Announcements', 'Official forum announcements and updates.', 7, '2024-06-01');

CREATE TABLE `Threads` (
  `thread_id` int NOT NULL AUTO_INCREMENT,
  `category_id` int NOT NULL,
  `thread_title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `thread_content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `created_by` int NOT NULL,
  `created_date` date NOT NULL,
  `view_count` int NOT NULL,
  `is_pinned` int NOT NULL,
  `is_closed` int NOT NULL,
  PRIMARY KEY (`thread_id`),
  KEY `IX_Threads_category_id` (`category_id`),
  CONSTRAINT `FK_Threads_Categories_category_id`
    FOREIGN KEY (`category_id`) REFERENCES `Categories` (`category_id`)
    ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=21
  DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `Threads`
  (`thread_id`, `category_id`, `thread_title`, `thread_content`,
   `created_by`, `created_date`, `view_count`, `is_pinned`, `is_closed`)
VALUES
  (1, 1, 'Welcome to the Forum!', 'Introduce yourself here and let’s get to know each other.', 1, '2024-01-15', 150, 1, 0),
  (2, 1, 'Forum Rules', 'Please read the forum rules before posting.', 1, '2024-01-16', 230, 1, 1),
  (3, 2, 'Best Programming Languages for Beginners', 'Which languages are best for beginners?', 1, '2024-02-01', 500, 0, 0),
  (4, 2, 'Java vs. Python', 'Debating the pros and cons of Java and Python.', 1, '2024-02-03', 600, 0, 0),
  (5, 3, 'HTML and CSS Tips', 'Share your best HTML and CSS tips here.', 1, '2024-02-15', 300, 0, 0),
  (6, 3, 'JavaScript Frameworks', 'Which framework is your favorite?', 1, '2024-02-20', 450, 0, 0),
  (7, 4, 'SQL Optimization Techniques', 'How do you optimize SQL queries?', 1, '2024-03-05', 350, 0, 0),
  (8, 4, 'NoSQL vs SQL', 'Discuss the differences and use cases.', 1, '2024-03-10', 400, 0, 0),
  (9, 5, 'Getting Started with Android Development', 'Tips for beginners in Android development.', 1, '2024-03-25', 200, 0, 0),
  (10, 5, 'Swift vs. Kotlin', 'Comparison of Swift and Kotlin for mobile development.', 1, '2024-04-01', 220, 0, 0),
  (11, 6, 'Best Practices for CI/CD', 'What are your best CI/CD practices?', 1, '2024-04-15', 320, 0, 0),
  (12, 6, 'Using Docker with Kubernetes', 'A guide to deploying applications.', 1, '2024-04-20', 410, 0, 0),
  (13, 7, 'Getting Started with Machine Learning', 'Where to start learning machine learning?', 1, '2024-05-01', 540, 0, 0),
  (14, 7, 'TensorFlow vs PyTorch', 'Which machine learning framework do you prefer?', 1, '2024-05-10', 580, 0, 0),
  (15, 8, 'Cybersecurity Best Practices', 'What are some essential practices?', 1, '2024-05-20', 480, 1, 0),
  (16, 8, 'Latest News in Cybersecurity', 'Discuss recent cybersecurity events.', 1, '2024-05-25', 450, 0, 0),
  (17, 9, 'Job Hunting Tips', 'Share your tips for finding a job in tech.', 1, '2024-06-01', 600, 0, 0),
  (18, 9, 'How to Prepare for Tech Interviews', 'Preparation strategies for interviews.', 1, '2024-06-05', 650, 0, 0),
  (19, 10, 'Forum Maintenance', 'Scheduled maintenance for forum upgrades.', 1, '2024-06-15', 300, 1, 1),
  (20, 10, 'New Features Announcement', 'Introducing new forum features!', 1, '2024-06-20', 450, 1, 1);

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES
  ('20241022024112_InitialCreate', '8.0.8'),
  ('20261002153002_AddCategoryThreadRelationshipSnakeCase', '8.0.8');

SET FOREIGN_KEY_CHECKS = 1;
