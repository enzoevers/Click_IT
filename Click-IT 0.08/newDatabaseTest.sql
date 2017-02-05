SELECT p.Username, p.birthday, g.Gender, s.Times_played, s.Best_time, s.Average_time 
FROM StatData s 
INNER JOIN Player p ON s.Player_Username_id = p.PlayerID 
INNER JOIN Gender g ON g.Id = p.Gender_id
WHERE s.Average_time IN (SELECT Average_Time FROM StatData WHERE Average_time > 0) 
ORDER BY s.Average_time

SELECT
	p.Username,    
    p.birthday,    
    g.Gender,    
    (SELECT 
		MAX(gd.GameID) 
     FROM	
		GameData gd
	 WHERE 
		p.PlayerID = gd.UserID
	 GROUP BY
		gd.UserID
	 HAVING    
		MAX(gd.GameID) > 0) AS Times_played,
		 
	(SELECT 
		MIN(gd.Reaction_time) 
     FROM	
		GameData gd
	 WHERE 
		p.PlayerID = gd.UserID
	 HAVING    
		MIN(gd.Reaction_time) > 0) AS Best,

    (SELECT 
		AVG(gd.Reaction_time)
     FROM	
		GameData gd
	 WHERE 
		p.PlayerID = gd.UserID
	 HAVING    
		AVG(gd.Reaction_time) > 0) AS Average 
FROM    
    StatData s
INNER JOIN    
    Player p ON s.Player_Username_id = p.PlayerID    
INNER JOIN    
    Gender g ON g.Id = p.Gender_id 
WHERE 
	(SELECT 
		MAX(gd.GameID) 
     FROM	
		GameData gd
	 WHERE 
		p.PlayerID = gd.UserID) > 0 
ORDER BY 
	(SELECT 
		MIN(gd.Reaction_time) 
	 FROM 
		GameData gd 
	 WHERE 
		p.PlayerID = gd.UserID) ASC

