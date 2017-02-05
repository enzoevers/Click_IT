SELECT 
	p.Username, 
	COUNT(Reaction_time) AS Times_played,
	AVG(Reaction_time) AS Average_time
FROM  
	GameData gb
INNER JOIN PLayers p 
	ON gb.UserID = p.PlayerID
GROUP BY 
	Username
HAVING 
	AVG(Reaction_time) <= 700
ORDER BY 
	AVG(Reaction_time) ASC


SELECT 
	p.Username, 
	min(gd.Reaction_time) AS Best_time 
FROM 
	GameData gd
INNER JOIN Players p 
	ON gd.UserID = p.PlayerID 
GROUP BY 
	p.Username 
ORDER BY 
	min(gd.Reaction_time) ASC


SELECT 
	avg(Reaction_Time) AS Average_time
FROM 
	GameData 
WHERE 
	UserID = (@UserID) 
	AND GameID = (@gameID)

SELECT 
	COUNT(*) AS Usernames_like_given_username
FROM	
	Players 
WHERE 
	Username LIKE 'enzeve809687'

SELECT 
	p.Username, 
	s.Player_age, 
	p.Gender, 
	s.Times_played, 
	s.Best_time, 
	s.Average_time
FROM 
	StatsData s 
INNER JOIN 
	Players p ON s.Player_Username_id = p.PlayerID 
WHERE 
	s.Average_time IN (SELECT Average_Time 
					   FROM StatsData 
					   WHERE Average_time > 0) 
ORDER BY 
	s.Average_time

SELECT 
	p.Username,
	COUNT(ld.LoginID) AS Times_logged_in
FROM 
	LoginData ld
INNER JOIN 
	Players p ON ld.UserID = p.PlayerID
WHERE Login_date IN (SELECT Login_date
					 FROM LoginData
					 WHERE Login_date <= '2016-05-15T00:00:00.000')
GROUP BY 
	p.Username	
HAVING 
	COUNT(ld.LoginID) >= 10
