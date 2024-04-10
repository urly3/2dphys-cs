static class Constants
{
    public static int fps = 60;
    public static float spf = 1.0f / fps;
    public static int ppm = 50;
    public static float gravity = 9.8f;
    public static Vec2d wind = new(0.6f * ppm, 0.0f);
}

class Force
{
    public static Vec2d generateDragForce(Particle p, float k)
    {
        Vec2d dragForce = new();

        if (p.velocity.lenSquared() <= 0)
        {
            return dragForce;
        }

        Vec2d dragDirection = p.velocity.unitVector().flip();

        float dragMagnitude = p.velocity.lenSquared() * k;

        dragForce = dragDirection.mul(dragMagnitude);

        return dragForce;
    }
}

class Particle
{
    public Vec2d position { get; set; } = new();
    public Vec2d velocity { get; set; } = new();
    public Vec2d acceleration { get; set; } = new();
    public Vec2d totalForce { get; set; } = new();

    public float mass { get; set; } = 1.0f;
    public float massInverse { get; set; } = 1.0f;
    public float elasticity { get; set; } = 1.0f;
    public int radius { get; set; } = 1;

    public Particle(float x, float y, float m, int r)
    {
        mass = m;
        position = new(x, y);
        radius = r;

        if (mass == 0)
            massInverse = 0;
        else
            massInverse = 1.0f / mass;
    }

    public Particle integrate(float deltaTime)
    {
        acceleration = totalForce.mul(massInverse);
        velocity.add(acceleration.mul(deltaTime));
        position.add(velocity.mul(deltaTime));
        clearForces();

        return this;
    }

    public Vec2d addForce(Vec2d force)
    {
        totalForce.add(force);
        return totalForce;
    }

    public Vec2d clearForces()
    {
        totalForce.x = 0;
        totalForce.y = 0;

        return totalForce;
    }

}

class Vec2d
{
    public float x { get; set; } = 0;
    public float y { get; set; } = 0;

    public Vec2d() { }
    public Vec2d(float _x, float _y)
    {
        x = _x;
        y = _y;
    }

    public float dot(Vec2d vec)
    {
        return (x * vec.x) + (y * vec.y);
    }

    public float cross(Vec2d vec)
    {
        return (x * vec.y) - (y * vec.x);
    }

    public float len()
    {
        return (float)Math.Sqrt((x * x) + (y * y));
    }

    public float lenSquared()
    {
        return (x * x) + (y * y);
    }

    public Vec2d add(Vec2d vec)
    {
        x += vec.x;
        y += vec.y;
        return this;
    }

    public Vec2d sub(Vec2d vec)
    {
        x -= vec.x;
        y -= vec.y;

        return this;
    }

    public Vec2d mul(float val)
    {
        return new(x * val, y * val);
    }

    public Vec2d scale(float val)
    {
        x *= val;
        y *= val;

        return this;
    }

    public Vec2d rotate(float angle)
    {
        return new((x * (float)Math.Cos(angle) - y * (float)Math.Sin(angle)),
            (x * (float)Math.Sin(angle) + (y * (float)Math.Cos(angle))));
    }

    public Vec2d div(float val)
    {
        x /= val;
        y /= val;

        return this;
    }

    public Vec2d flip()
    {
        x = -x;
        y = -y;

        return this;
    }

    public Vec2d flipped()
    {
        return new(-x, -y);
    }

    public Vec2d normalise()
    {
        float length = len();

        if (length == 0)
        {
            return this;
        }

        x /= length;
        y /= length;

        return this;
    }

    public Vec2d unitVector()
    {
        float length = len();

        if (length == 0)
        {
            return new();
        }

        return new(x / length, y / length);
    }

    public Vec2d normal()
    {
        float length = len();
        return new(x / length, y / length);
    }
}
