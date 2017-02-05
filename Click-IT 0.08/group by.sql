Select 
		UserID,
		avg(Reaction_time)
FROM 
		GameData
group by
		UserID
having 
		UserID = 15

