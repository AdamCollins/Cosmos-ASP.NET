CREATE TABLE posys(
	id INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
	textContent TEXT,
	imageContent IMAGE,
	postDateTime DATETIME
);
CREATE TABLE forum_comments(
	id INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
	post_id INTEGER NOT NULL,
	text_content TEXT,
	post_datetime DATETIME
);