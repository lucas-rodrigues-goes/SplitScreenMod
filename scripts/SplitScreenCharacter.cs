using BmSDK;
using BmSDK.BmGame;
using BmSDK.BmScript;
using BmSDK.Engine;

// Adds support for swapping characters based on mods.toml
[Script]
public class SplitScreenCharacter : Script
{
    [ScriptComponent(AutoAttach = true)]
    class GameRIComponent : ScriptComponent<RGameRI>
    {
        [ComponentRedirect(nameof(RGameRI.PreBeginPlay))]
        public void PreBeginPlay()
        {
            Owner.PreBeginPlay();

            var playerConfigs = (TomlArray)SplitScreen.Instance.Options["players"];
            for (var i = 0; i < playerConfigs.Count; i++)
            {
                var playerConfig = (TomlArray)playerConfigs[i]!;
                Owner.PlayerCharacters[i + 1] = new RGameRI.FLoadedPlayerCharacter()
                {
                    CharacterName = (string)playerConfig[0]!,
                };
            }
        }
    }

    [ScriptComponent(AutoAttach = true)]
    class GameInfoComponent : ScriptComponent<RGameInfo>
    {
        [ComponentRedirect(nameof(RGameInfo.GetPlayerCharacterIndex))]
        public int GetPlayerCharacterIndex(Controller C)
        {
            if (C is RPlayerController rpc && rpc.IsSplitscreenPlayer(out _))
            {
                return rpc.GetMultiplayerIndex();
            }

            return Owner.GetPlayerCharacterIndex(C);
        }
    }
}
