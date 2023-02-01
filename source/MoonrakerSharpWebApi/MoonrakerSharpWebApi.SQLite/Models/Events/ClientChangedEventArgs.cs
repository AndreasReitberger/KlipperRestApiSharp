namespace AndreasReitberger.API.Moonraker.Models.Events
{
    public class ClientChangedEventArgs : EventArgs
    {
        #region Properties
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        #endregion
    }
}
