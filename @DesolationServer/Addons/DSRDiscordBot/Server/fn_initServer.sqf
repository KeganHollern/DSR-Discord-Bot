/*
	Server Bot Handler
*/
_serverName = ["Server_Name","DDB"] call BASE_fnc_getCfgValue;
_secretKey = ["Bot_Token","DDB"] call BASE_fnc_getCfgValue;

diag_log "Discord Bot  > Initializing";

diag_log _serverName;
diag_log _secretKey;
_resp = "DSRDiscordBot" callExtension ["start",[_secretKey,_serverName]];

if((_resp select 1) == 100) then {
	while{true} do {
		_resp = "DSRDiscordBot" callExtension ["request",[]];

		_type = _resp select 1;
		
		if(_type == 0) then { // !perforamance
			"DSRDiscordBot" callExtension ["response",[_type,diag_fps,diag_activeScripts select 0]];
		};
		if(_type == 1) then { // !statistics
			"DSRDiscordBot" callExtension ["response",[_type,floor(diag_tickTime / 60),count(allPlayers),count(allUnits)-count(allPlayers)]];
		};
		if(_type == 2) then { // !shutdown
			"DSRDiscordBot" callExtension ["response",[_type]];
			
		};
		if(_type == 3) then {
			_names = [];
			{
				_names pushback name _x;
				true
			} count allPlayers;
			_msg = _names joinString "`";
			"DSRDiscordBot" callExtension ["response",[_type,_msg]];
		};
		uiSleep 0.5;
	};
} else {
	diag_log "Some error!";
};