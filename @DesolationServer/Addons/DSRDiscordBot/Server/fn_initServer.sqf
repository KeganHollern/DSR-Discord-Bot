/*
	Server Bot Handler
*/
_serverName = ["Server_Name","DDB"] call BASE_fnc_getCfgValue;
_secretKey = ["Bot_Token","DDB"] call BASE_fnc_getCfgValue;

diag_log "Discord Bot  > Initializing";

diag_log _serverName;
diag_log _secretKey;
_resp = "DSRDiscordBot" callExtension ["start",[_secretKey,_serverName]];

DDB_var_CPS = 0;
[] spawn {
    while{true} do {
        _cps = 0;
        _lTime = diag_tickTime;
        waitUntil{_cps = _cps + 1; diag_tickTime >= (_lTime + 1)};
        DDB_var_CPS = _cps;
    };
};

if((_resp select 1) == 100) then {
	while{true} do {
		_resp = "DSRDiscordBot" callExtension ["request",[]];

		_type = _resp select 1;
		
		if(_type == 0) then { // !perforamance
			"DSRDiscordBot" callExtension ["response",[_type,diag_fps,diag_activeScripts select 0,DDB_var_CPS]];
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