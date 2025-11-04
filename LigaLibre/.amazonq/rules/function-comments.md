# Comentarios de Funciones

Al crear o modificar funciones, métodos o propiedades, agregar comentarios XML documentation (///).

## Formato

### Métodos públicos:
```csharp
/// <summary>
/// Descripción del método
/// </summary>
/// <param name="parametro">Descripción del parámetro</param>
/// <returns>Descripción del retorno</returns>
```

### Métodos privados:
```csharp
/// <summary>
/// Descripción del método
/// </summary>
```

## Ejemplos

```csharp
/// <summary>
/// Obtiene todos los jugadores del sistema
/// </summary>
/// <returns>Colección de jugadores en formato DTO</returns>
public async Task<IEnumerable<PlayerDto>> GetAllPlayers()

/// <summary>
/// Actualiza un jugador existente
/// </summary>
/// <param name="id">ID del jugador</param>
/// <param name="createPlayerDto">Datos del jugador</param>
/// <returns>Jugador actualizado</returns>
/// <exception cref="ArgumentException">Cuando el jugador no existe</exception>
public async Task<PlayerDto> UpdatePlayerAsync(int id, CreatePlayerDto createPlayerDto)
```

Usa español y sé conciso.
