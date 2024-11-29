using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface[] navMeshSurfaces; // Array de superficies NavMesh de diferentes escenarios
    private NavMeshData[] savedNavMeshData; // Para almacenar los datos de cada NavMesh

    void Start()
    {
        // Inicializar la estructura para guardar los datos
        savedNavMeshData = new NavMeshData[navMeshSurfaces.Length];

        // Guardar los datos de cada superficie inicial
        for (int i = 0; i < navMeshSurfaces.Length; i++)
        {
            SaveNavMesh(i);
        }
    }

    // Método para guardar los datos de un NavMesh específico
    public void SaveNavMesh(int index)
    {
        if (index < 0 || index >= navMeshSurfaces.Length) return;

        // Crear un nuevo objeto NavMeshData y actualizarlo
        NavMeshData navData = new NavMeshData();
        navMeshSurfaces[index].BuildNavMesh();
        navMeshSurfaces[index].navMeshData = navData;

        // Guardar los datos en el array
        savedNavMeshData[index] = navData;
    }

    // Método para cargar un NavMesh en tiempo de ejecución
    public void LoadNavMesh(int index)
    {
        if (index < 0 || index >= savedNavMeshData.Length) return;

        // Eliminar datos previos si es necesario
        NavMesh.RemoveAllNavMeshData();

        // Agregar los datos almacenados al NavMesh
        if (savedNavMeshData[index] != null)
        {
            NavMesh.AddNavMeshData(savedNavMeshData[index]);
        }
    }
}
