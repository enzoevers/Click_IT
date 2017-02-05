SElECT p.Username, p.First_name, p.Gender, s.Player_age, s.Times_played, s.Best_time, s.Average_time
FROM StatsData s
INNER JOIN Players p 
ON s.Player_Username_id = p.PlayerID

