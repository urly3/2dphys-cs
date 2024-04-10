using Raylib_cs;

//////////////////////////////////////////

Raylib.InitWindow(1920, 1080, "window");
Raylib.ToggleFullscreen();

List<Particle> particles = new();
Vec2d pushForce = new();
float timeLast = 0;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);


    input();
    update();
    render();

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

//////////////////////////////////////////

void input()
{
    pushForce.x = 0;
    pushForce.y = 0;

    if (Raylib.IsKeyDown(KeyboardKey.Up)) pushForce.y -= 50 * Constants.ppm;
    if (Raylib.IsKeyDown(KeyboardKey.Down)) pushForce.y += 50 * Constants.ppm;
    if (Raylib.IsKeyDown(KeyboardKey.Left)) pushForce.x -= 50 * Constants.ppm;
    if (Raylib.IsKeyDown(KeyboardKey.Right)) pushForce.x = 50 * Constants.ppm;

    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        int mouseX = Raylib.GetMouseX();
        int mouseY = Raylib.GetMouseY();
        int radius = 1;
        float mass = 1;
        int screenWidth = Raylib.GetScreenWidth();

        if (mouseX < screenWidth / 3)
        {
            radius = 3;
            mass = 1.0f;
        }
        else if (mouseX < screenWidth / 1.5 &&
              mouseX > screenWidth / 3)
        {
            radius = 6;
            mass = 2.0f;
        }
        else
        {
            radius = 12;
            mass = 4.0f;
        }

        particles.Add(new(mouseX, mouseY, mass, radius));
    }
}

void update()
{
    float timeToWait = Constants.spf - ((float)Raylib.GetTime() - timeLast);

    if (timeToWait > 0.0f)
    {
        Raylib.WaitTime(timeToWait);
    }

    float deltaTime = (float)Raylib.GetTime() - timeLast;

    Raylib.DrawText(((int)(1.0f / Raylib.GetFrameTime())).ToString(), 12, 12, 20, Color.Black);

    if (deltaTime > 0.0166f)
    {
        deltaTime = 0.0166f;
    }

    timeLast = (float)Raylib.GetTime();

    foreach (var particle in particles)
    {
        int width = Raylib.GetScreenWidth();
        int height = Raylib.GetScreenHeight();
        Vec2d weight = new(0.0f, particle.mass * Constants.gravity * Constants.ppm);
        particle.addForce(Constants.wind);
        particle.addForce(weight);
        particle.addForce(pushForce);

        if (particle.position.y >= height / 2)
        {
            particle.addForce(Force.generateDragForce(particle, 0.01f));
        }

        particle.integrate(deltaTime);

        if (particle.position.y > height - particle.radius)
        {
            particle.position.y = height - particle.radius;
            particle.velocity.y *= -particle.elasticity;
        }
        else if (particle.position.y < particle.radius)
        {
            particle.position.y = particle.radius;
            particle.velocity.y *= -particle.elasticity;
        }

        if (particle.position.x > width - particle.radius)
        {
            particle.position.x = width - particle.radius;
            particle.velocity.x *= -particle.elasticity;
        }
        else if (particle.position.x < particle.radius)
        {
            particle.position.x = particle.radius;
            particle.velocity.x *= -particle.elasticity;
        }
    }
}

void render()
{
    int width = Raylib.GetScreenWidth();
    int height = Raylib.GetScreenHeight();
    Raylib.ClearBackground(Color.Gray);

    Raylib.DrawRectangle(0, height / 2, width, height / 2, Color.Blue);
    foreach (var particle in particles)
    {
        Raylib.DrawCircle((int)particle.position.x, (int)particle.position.y, particle.radius, Color.Red);
    }
}
