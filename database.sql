DROP TABLE forum_posts;
CREATE TABLE forum_posts(
	id INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
	textContent NVARCHAR(1000),
	imageContent IMAGE,
	postDateTime DATETIME
);
DROP TABLE forum_comments;
CREATE TABLE forum_comments(
	id INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
	post_id INTEGER NOT NULL,
	text_content NVARCHAR(500),
	post_datetime DATETIME
);

DROP TABLE forum_users;
CREATE TABLE forum_users(
	id INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
	username VARCHAR(12),
	hashed_password VARCHAR(56),
	salt BINARY(64),
	create_date DATETIME
	
);