namespace Tools.Interfaces
{ 
    // ¿porque hay una interfaz de esto?
    // porque cada vez que el checkpoint manager de turno
    // me avise que el player irá al untimo checkpoint...
    // cada script que implemente esto por ejemplo...
    // un manager de enemigos puede hacer que todos los enemigos
    // vuelvan a su posicion original
public interface ICheckpointReseteable
    {
        void CheckpointReset();
    }
}

