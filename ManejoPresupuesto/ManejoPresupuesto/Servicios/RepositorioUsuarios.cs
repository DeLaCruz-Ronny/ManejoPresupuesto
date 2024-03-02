namespace ManejoPresupuesto.Servicios
{

    public interface IServicioUsuarios
    {
        int ObtenerUsuariosId();
    }

    public class RepositorioUsuarios : IServicioUsuarios
    {
        public int ObtenerUsuariosId()
        {
            return 1;
        }
    }
}
