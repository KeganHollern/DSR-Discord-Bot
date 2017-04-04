/*
	Example Plugin
	
	Configuration Layout

*/
class CfgPatches
{
	class DSRDiscordBot  // PBO PREFIX HERE
	{
		requiredAddons[] = {"PluginManager"};
	}; 
};
// Plugin Entry Config
class Plugins
{
	class DSRDiscordBot // PBO PREFIX HERE
	{
		name = "Discord Bot"; //plugin name
		desc = "Adds discord functionality to your desolation server"; //plugin description
		tag = "DDB"; //plugin function tag (used in cfgFunctions)
	};
};
// Plugin Functions
class CfgFunctions
{
	class DDB
	{
		class Server 
		{
			file = "DSRDiscordBot\Server";
			isserver = 1;
			class initServer {};
		};
	};
};



