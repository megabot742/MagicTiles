# Candy Piano Tiles - Magic Tiles Clone

A rhythm-based piano tiles game inspired by Magic Tiles, built with Unity.

## How to Run

1. **Open the Project**
   - Open Unity Hub → Add project → Select the project folder.
   - Make sure you are using **Unity 2022.3.62f3** (recommended).

2. **Open the Scene**
   - In the Project window, navigate to `Assets/Scenes/`.
   - Open the scene named **`MainMenu`** (or the main scene you set as starting scene).

3. **Play the Game**
   - Press the **Play** button (▶) at the top of the Unity Editor.
   - Use mouse or touch input to tap the falling tiles according to the music.

**Note**: The game starts from the song list screen. Select any song and choose Normal or Bomb mode to play.

## Design Choices

- **Architecture**: Used Singleton pattern for core managers (`GameController`, `AudioController`, `GameView`) to keep the project simple and easy to manage for a small-scale rhythm game.
- **Node System**: Each falling tile is a separate `Node` prefab with different types (Normal, Long, Bomb, Start). Long notes use a growing bar mechanic with perfect timing detection.
- **Audio Handling**: Centralized audio management in `AudioController` with separate logic for background music and sound effects.
- **Progression**: Speed gradually increases every 30 points, combined with background changes to create a sense of progression and increasing difficulty.
- **UI**: Canvas-based UI with simple state management (Home → Game Mode → Gameplay → Game Over).

The project focuses on core gameplay feel (timing, hold mechanics, scoring) while keeping the codebase maintainable.

## AI Usage

- Used **Grok (xAI)** extensively throughout the development process.
- Grok helped with code refactoring, debugging complex logic (especially Long Note hold mechanics and scoring), performance suggestions, and structuring clean, maintainable code.
- Significant assistance in optimizing `Node.cs` and `GameView.cs`.

## Asset Attributions

- UI templates and some graphical assets were taken from external Unity asset packages / templates.
- Background music and sound effects are placeholder tracks (piano notes).
- All custom code and game logic were written from scratch with AI assistance.

## Known Issues & Limitations

- **Performance**: No Object Pooling implemented yet for nodes → may experience lag after playing for a long time.
- **Note System**: Currently uses random generation instead of precise note charts from music (planned improvement).
- **Input**: Tap detection can occasionally miss on mobile devices (needs improvement with better lane-based input).
- **Long Note**: Hold mechanic is functional but still needs fine-tuning for better feel and visual feedback.
- **Optimization**: Overall project is not fully optimized for mobile (draw calls, GC, etc.).

## Future Improvements

- Implement Object Pooling for nodes
- Add proper song charting system (timing-based notes)
- Improve mobile input and touch handling
- Add more visual effects and polish
- Optimize performance for mobile devices
