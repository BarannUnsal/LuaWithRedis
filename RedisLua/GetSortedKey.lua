local key = @key1
local redis_db = @key2

redis.call("select", redis_db)

local responses = {}

local scores = redis.call("zrange", key, 0, 9, 'withscores')

for i = 1, #scores, 2 do
	table.insert(responses, {Member = scores[i], Score = scores[i + 1]})
end

local json_Responses = cjson.encode(responses)

return json_Responses