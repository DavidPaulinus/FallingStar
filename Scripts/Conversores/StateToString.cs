public class StateToString 
{
    public static string ConvertState(string state)
    {
        int _stringIndexStart = state.IndexOf("(");

        var _state = state.Substring(_stringIndexStart + 1);

        var _indexState = _state.IndexOf("State");

        return _state.Substring(0, _indexState).Trim();
    }
}
