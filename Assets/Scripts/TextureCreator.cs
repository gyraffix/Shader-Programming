using UnityEngine;


public class TextureCreator : MonoBehaviour {
	// Add your own pattern types here:
	public enum PatternType { 
		Noise = 0,
        None = 1,
        Mandelbrot = 2,
        ColorNoise = 3,
        UVIndex = 4,
        GreyscaleBars = 5,
        Checkerboard = 6,
        YellowGreenStripes = 7,
        RedCircle = 8,
        FadingCircle = 9,
        Ring = 10,
        Curtains = 11,
        CosFunction = 12,
        PerlinNoise = 13,
        InterpolationPerlin = 14,
        Rainbow = 15
    };

    public enum RotationPoint { Origin, Center }

    public PatternType patternType;
	[Range(0,360)] public float rotation;
	
	public RotationPoint rotationPoint;
	const int SIZE = 1024;

	Texture2D texture = null;
	Color[] cols = null;

	void Start() {
		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;

		Draw();
	}

	public Vector2 RotateVector(Vector2 vector, float degrees, RotationPoint rp)
	{
		degrees = Mathf.Deg2Rad * degrees;
		switch (rp)
		{
			case RotationPoint.Origin:
				return new Vector2(
			(int)(vector.x * Mathf.Cos(degrees) - vector.y * Mathf.Sin(degrees)),
			(int)(vector.x * Mathf.Sin(degrees) + vector.y * Mathf.Cos(degrees)));
			case RotationPoint.Center:
				Vector2 origin = new Vector2((int)(SIZE / 2), (int)(SIZE / 2));
				vector = vector - origin;
				vector = new Vector2(
            (int)(vector.x * Mathf.Cos(degrees) - vector.y * Mathf.Sin(degrees)),
            (int)(vector.x * Mathf.Sin(degrees) + vector.y * Mathf.Cos(degrees)));

                return vector + origin;
			default: return new Vector2(0, 0);
		}
		
	}

	/// <summary>
	/// Returns the pixel color for texture coordinate (u,v), for a given pattern.
	/// </summary>
	Color CalculatePixelColor(float u, float v, PatternType pattern) {
		// TODO: insert your own pattern creation code here.
		//  See the slides for details.
		switch (pattern) {
			case PatternType.Noise: // white noise				
				return Random.value * Color.white;
			case PatternType.Mandelbrot:
				return Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f));
			case PatternType.ColorNoise:
				return new Color(Random.value, Random.value, Random.value, 1);
			case PatternType.UVIndex:
				return new Color(u,v,0,1);
			case PatternType.GreyscaleBars:
				return Color.white * Mathf.Floor(u / 0.125f % 2);
			case PatternType.Checkerboard:
                return Color.white * ((Mathf.Floor(u / 0.125f % 2) + Mathf.Floor(v / 0.125f % 2)) % 2);
			case PatternType.YellowGreenStripes:
				float t = (u / 0.125f + v / 0.25f) % 1; // TODO: change angle...
				Color col1 = new Color(1, 1, 0, 1);
				Color col2 = new Color(0, 1, 0, 1);
				Color col3 = new Color(0, 0, 0, 1);
				if (t <= 0.5)
					return col1 + (col2 - col1) * (t * 2); // linear interpolation formula!
				else return col2 + (col3 - col2) * ((t - 0.5f) * 2);
			//return new Color(t,1,0,1); // TODO: use t to go from yellow to green-blackish
			case PatternType.RedCircle:
				float circleVar = Mathf.Sqrt(Mathf.Pow(0.5f - u, 2) + Mathf.Pow(0.5f - v, 2));

                if (circleVar > 0.5)
				{
					return Color.black;
				}
				else return Color.red;
			case PatternType.FadingCircle:
				float fadingVar = Mathf.Sqrt(Mathf.Pow(0.5f - u, 2) + Mathf.Pow(0.5f - v, 2));

                if (fadingVar > 0.5)
				{
					return Color.black;
				}
				else
				{
					return Color.magenta + (Color.black - Color.magenta * fadingVar * 2);
				}
			case PatternType.Ring:
                float ringVar = Mathf.Sqrt(Mathf.Pow(0.5f - u, 2) + Mathf.Pow(0.5f - v, 2));

                if (ringVar >= 0.425 && ringVar <= 0.45)
                {
                    return Color.magenta + (Color.black - Color.magenta * ((ringVar - 0.425f)*40));
                }
				else if (ringVar >= 0.4 && ringVar < 0.425)
				{
                    return Color.magenta + (Color.black - Color.magenta * (1 - (ringVar - 0.4f) * 40));
                }
                else
                {
					return Color.black;
                }
			case PatternType.Curtains:
				return new Color(Mathf.Cos(u*Mathf.PI*2) * 0.5f +0.5f, 0, 0, 1);
			case PatternType.CosFunction:
				if (Mathf.Cos(u * Mathf.PI * 2) * 0.5f + 0.5f < v)
					return Color.black;
				else return Color.red;
			case PatternType.PerlinNoise:
				return Color.white * Mathf.PerlinNoise(u * 10, v * 10);
			case PatternType.InterpolationPerlin:
				return Color.yellow + (new Color(0.5f,0.25f,0,1) - Color.yellow * Mathf.PerlinNoise(u * 40, v * 8));
			case PatternType.Rainbow:
				float rainbowVar = u * 6;
				if (rainbowVar < 1)
				{
					return Color.magenta + ((Color.red - Color.magenta) * rainbowVar);
				}
				else if (rainbowVar >= 1 && rainbowVar < 2)
				{
                    return Color.red + ((Color.yellow - Color.red) * (rainbowVar-1));
                }
                else if (rainbowVar >= 2 && rainbowVar < 3)
                {
                    return Color.yellow + ((Color.green - Color.yellow) * (rainbowVar - 2));
                }
                else if (rainbowVar >= 3 && rainbowVar < 4)
                {
                    return Color.green + ((Color.cyan - Color.green) * (rainbowVar - 3));
                }
                else if (rainbowVar >= 4 && rainbowVar < 5)
                {
                    return Color.cyan + ((Color.blue - Color.cyan) * (rainbowVar - 4));
                }
                else
                {
                    return Color.blue + ((Color.magenta - Color.blue) * (rainbowVar-5));
                }
				
            default:
			
				return Color.blue;
		}
	}

	/// <summary>
	/// Draws a pattern given by the [pattern] number to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] cols, int width, int height, PatternType pattern, RotationPoint rp) {
		for (int x = 0; x < width; x++) {
			// TODO: calculate UV coordinates and pass them to CalculatePixelColor:
			for (int y = 0; y < height; y++)
			{
				Vector2 uv = RotateVector(new Vector2(x, y), rotation, rp);

                cols[y * width + x] = CalculatePixelColor((float)uv.x /width, (float) uv.y/height, pattern);
            }
			
		}
	}

	void Draw() {
		if (cols == null) {
			cols = texture.GetPixels();
		}
		DrawPattern(cols, SIZE, SIZE, patternType, rotationPoint);

		texture.SetPixels(cols);
		texture.Apply();
	}

	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate() {
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null) return;
		Draw();
	}

	private void nextPainting()
	{
		if ((int)patternType == 15)
		{
			patternType = 0;
		}
		else patternType++;
    }

	private void Update() {
		// Control + S saves to file:
		if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			var exporter = GetComponent<TextureExporter>();
			if (exporter != null) {
				exporter.ExportTexture(texture);
			}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			nextPainting();
			Draw();
		}

	}

	#region Mandelbrot
	// Used for the Mandelbrot fractal:
	const int maxIterations = 30;
	const float escapeLengthSquared = 4;

	Color Mandelbrot(float cReal, float cImaginary) {
		int iteration = 0;

		float zReal = 0;
		float zImaginary = 0;

		while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations) {
			// Use Mandelbrot's magic iteration formula: z := z^2 + c 
			// (using complex number multiplication & addition - 
			//   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
			float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
			zImaginary = 2 * zReal * zImaginary + cImaginary;
			zReal = newZr;
			iteration++;
		}
		// Return a color value based on the number of iterations that were needed to "escape the circle":
		float grad = 1f * iteration / maxIterations; // between 0 and 1
													 // TODO: use a nicer gradient
		return new Color(grad, grad, grad);
	}
	#endregion
}
