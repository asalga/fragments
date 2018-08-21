// Parameters
#define VOXEL_RESOLUTION  4.
#define VOXEL_LIGHTING
#define SHADOW
#define MOUSE
#define HSV2RGB_FAST

#define CAMERA_FOCAL_LENGTH 2.0
#define DELTA       0.01
#define RAY_LENGTH_MAX    500.0
#define RAY_STEP_MAX    100.0
#define AMBIENT       0.2
#define SPECULAR_POWER    2.0
#define SPECULAR_INTENSITY  0.3
#define SHADOW_LENGTH   150.0
#define SHADOW_POWER    3.0
#define FADE_POWER      1.0
#define BACKGROUND      0.7
#define GLOW        0.4
#define GAMMA       0.8

// Math constants
#define PI    3.14159265359
#define SQRT3 1.73205080757

// Global variable to handle the glow effect
float glowCounter;

// PRNG (from https://www.shadertoy.com/view/4djSRW)
float rand (in vec3 seed) {
  seed = fract (seed * vec3 (5.3983, 5.4427, 6.9371));
  seed += dot (seed.yzx, seed.xyz + vec3 (21.5351, 14.3137, 15.3219));
  return fract (seed.x * seed.y * seed.z * 95.4337);
}

// Distance to the voxel
float distVoxel (in vec3 p) {

  // Update the glow counter
  //++glowCounter;

  // Rounded box
  const float voxelRadius = 0.25;
  return length (max (abs (p) - 0.5 + voxelRadius, 0.0)) - voxelRadius;
}

// Distance to the scene and color of the closest point
vec2 distScene (in vec3 p, out vec3 P) {

  // Scaling
  p *= VOXEL_RESOLUTION;

  // Velocity, period of the waves, spacing of the gums
  float v = VOXEL_RESOLUTION * floor (iTime * 100.0 / VOXEL_RESOLUTION);
  const float k1 = 0.05;
  const float k2 = 60.0;

  // Giant Pac-Man
  float body = length (p);
  body = max (body - 32.0, 27.0 - body);
  float eyes = 6.0 - length (vec3 (abs (p.x) - 12.5, p.y - 19.5, p.z - 20.0));
  float mouthAngle = PI * (0.07 + 0.07 * cos (2.0 * v * PI / k2));
  float mouthTop = dot (p, vec3 (0.0, -cos (mouthAngle), sin (mouthAngle))) - 2.0;
  mouthAngle *= 2.5;
  float mouthBottom = dot (p, vec3 (0.0, cos (mouthAngle), sin (mouthAngle)));

    float pacMan = body;

    vec2 d = vec2 (pacMan, 0.13);
  P = p;

  // Gums
  vec3 q = vec3 (p.xy, mod (p.z + v, k2) - k2 * 0.5);
  float gum = max (length (q) - 6.0, -p.z);
  if (gum < d.x) {
    d = vec2 (gum, 0.35);
    P = q;
  }



  // Scaling
  d.x /= VOXEL_RESOLUTION;
  return d;
}

// Distance to the (voxelized?) scene
vec4 dist (inout vec3 p, in vec3 ray, in float voxelized, in float rayLengthMax) {
  vec3 P = p;
  vec2 d = vec2 (1.0 / 0.0, 0.0);
  float rayLength = 0.0;
  float rayLengthInVoxel = 0.0;
  float rayLengthCheckVoxel = 0.0;
  vec3 raySign = sign (ray);
  vec3 rayDeltaVoxel = raySign / ray;
  for (float rayStep = 0.0; rayStep < RAY_STEP_MAX; ++rayStep) {
    if (rayLength < rayLengthInVoxel) {
      d.x = distVoxel (fract (p + 0.5) - 0.5);
      if (d.x < DELTA) {
        break;
      }
    } else if (rayLength < rayLengthCheckVoxel) {
      vec3 rayDelta = (0.5 - raySign * (fract (p + 0.5) - 0.5)) * rayDeltaVoxel;
      float dNext = min (rayDelta.x, min (rayDelta.y, rayDelta.z));
      d = distScene (floor (p + 0.5), P);
      if (d.x < 0.0) {
        rayDelta = rayDeltaVoxel - rayDelta;
        d.x = max (rayLengthInVoxel - rayLength, DELTA - min (rayDelta.x, min (rayDelta.y, rayDelta.z)));
        rayLengthInVoxel = rayLength + dNext;
      } else {
        d.x = DELTA + dNext;
      }
    } else {
      d = distScene (p, P);
      if (voxelized > 0.5) {
        if (d.x < SQRT3 * 0.5) {
          rayLengthCheckVoxel = rayLength + abs (d.x) + SQRT3 * 0.5;
          d.x = max (rayLengthInVoxel - rayLength + DELTA, d.x - SQRT3 * 0.5);
        }
      } else if (d.x < DELTA) {
        break;
      }
    }
    rayLength += d.x;
    if (rayLength > rayLengthMax) {
      break;
    }
    p += d.x * ray;
  }
  return vec4 (d, rayLength, rand (P));
}

// Normal at a given point
vec3 normal (in vec3 p, in float voxelized) {
  vec2 h = vec2 (DELTA, -DELTA);
  vec3 n;
  //if (voxelized > 0.5) {
    p = fract (p + 0.5) - 0.5;
    n = h.xxx * distVoxel (p + h.xxx) +
      h.xyy * distVoxel (p + h.xyy) +
      h.yxy * distVoxel (p + h.yxy) +
      h.yyx * distVoxel (p + h.yyx);
  //} else {
  //  n = h.xxx * distScene (p + h.xxx, n).x +
  //    h.xyy * distScene (p + h.xyy, n).x +
  //    h.yxy * distScene (p + h.yxy, n).x +
  //    h.yyx * distScene (p + h.yyx, n).x;
  //}
  return normalize (n);
}

// HSV to RGB
vec3 hsv2rgb (in vec3 hsv) {
  #ifdef HSV2RGB_SAFE
  hsv.yz = clamp (hsv.yz, 0.0, 1.0);
  #endif
  #ifdef HSV2RGB_FAST
  return hsv.z * (1.0 + 0.5 * hsv.y * (cos (2.0 * PI * (hsv.x + vec3 (0.0, 2.0 / 3.0, 1.0 / 3.0))) - 1.0));
  #else
  return hsv.z * (1.0 + hsv.y * clamp (abs (fract (hsv.x + vec3 (0.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 2.0, -1.0, 0.0));
  #endif
}

// Main function
void mainImage (out vec4 fragColor, in vec2 fragCoord) {

  // Get the fragment
  vec2 frag = (2.0 * fragCoord.xy - iResolution.xy) / iResolution.y;

  // Define the rendering mode
  float modeTiming = iTime * 0.234;
  float modeAngle = PI * cos (iTime * 0.2);
  modeAngle = dot (frag - vec2 (cos (iTime * 2.0), 0.0), vec2 (cos (modeAngle), sin (modeAngle)));
  float modeVoxel = step (0.5, fract (modeTiming / (4.0 * PI)));
  modeTiming = cos (modeTiming);
  float mode3D = smoothstep (0.8, 0.5, modeTiming);
  float modeSwitch = smoothstep (0.995, 1.0, modeTiming) + smoothstep (0.02, 0.0, abs (modeAngle)) * (1.0 - modeVoxel);
  modeVoxel += step (0.0, modeAngle) * (1.0 - modeVoxel);

  // Define the ray corresponding to this fragment
  vec3 ray = normalize (vec3 (frag, mix (8.0, CAMERA_FOCAL_LENGTH, mode3D)));

  // Compute the orientation of the camera
  float yawAngle = PI * (1.2 + 0.2 * cos (iTime * 0.5));
  float pitchAngle = PI * (0.1 * cos (iTime * 0.3) - 0.05);
  #ifdef MOUSE
  yawAngle += 4.0 * PI * iMouse.x / iResolution.x;
  pitchAngle += PI * 0.3 * (1.0 - iMouse.y / iResolution.y);
  #endif
  yawAngle = mix (PI * 1.5, yawAngle, mode3D);
  pitchAngle *= mode3D;

  float cosYaw = cos (yawAngle);
  float sinYaw = sin (yawAngle);
  float cosPitch = cos (pitchAngle);
  float sinPitch = sin (pitchAngle);

  mat3 cameraOrientation;
  cameraOrientation [0] = vec3 (cosYaw, 0.0, -sinYaw);
  cameraOrientation [1] = vec3 (sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
  cameraOrientation [2] = vec3 (sinYaw * cosPitch, -sinPitch, cosYaw * cosPitch);

  ray = cameraOrientation * ray;

  // Compute the origin of the ray
  float cameraDist = mix (300.0, 95.0 + 50.0 * cos (iTime * 0.8), mode3D);
  vec3 origin = (vec3 (0.0, 0.0, 40.0 * sin (iTime * 0.2)) - cameraOrientation [2] * cameraDist) / VOXEL_RESOLUTION;

  // Compute the distance to the scene
  glowCounter = 0.0;
  vec4 d = dist (origin, ray, modeVoxel, RAY_LENGTH_MAX / VOXEL_RESOLUTION);

  // Set the background color
  vec3 finalColor = hsv2rgb (vec3 (0.2 * ray.y + 0.4 * modeVoxel - 0.37, 1.0, mode3D * BACKGROUND));
  vec3 glowColor = GLOW * vec3 (1.0, 0.3, 0.0) * glowCounter / RAY_STEP_MAX;
  if (d.x < DELTA) {

    // Set the object color
    vec3 color = hsv2rgb (vec3 (d.y + 0.1 * d.w * modeVoxel, 0.5 + 0.5 * modeVoxel, 1.0));

    // Lighting
    vec3 l = normalize (mix (vec3 (1.0, 0.0, 0.0), vec3 (1.25 + cos (iTime * 0.2), 1.0, 1.0), mode3D));
    #ifdef VOXEL_LIGHTING
    if (modeVoxel > 0.5) {
      vec3 n = normal (floor (origin + 0.5), 0.0);
      float diffuse = max (0.0, dot (n, l));
      float specular = pow (max (0.0, dot (reflect (ray, n), l)), SPECULAR_POWER) * SPECULAR_INTENSITY;
      color = (AMBIENT + diffuse) * color + specular;
    }
    #endif
    vec3 n = normal (origin, modeVoxel);
    float diffuse = dot (n, l);
    float specular;
    if (diffuse < 0.0) {
      diffuse = 0.0;
      specular = 0.0;
    } else {
      specular = pow (max (0.0, dot (reflect (ray, n), l)), SPECULAR_POWER) * SPECULAR_INTENSITY;
      #ifdef SHADOW
      origin += n * DELTA * 2.0;
      vec4 shadow = dist (origin, l, modeVoxel, SHADOW_LENGTH / VOXEL_RESOLUTION);
      if (shadow.x < DELTA) {
        shadow.z = pow (min (1.0, shadow.z * VOXEL_RESOLUTION / SHADOW_LENGTH), SHADOW_POWER);
        diffuse *= shadow.z;
        specular *= shadow.z;
      }
      #endif
    }
    color = (AMBIENT + diffuse) * color + specular;

    // Fading
    float fade = pow (max (0.0, 1.0 - d.z * VOXEL_RESOLUTION / RAY_LENGTH_MAX), FADE_POWER);
    finalColor = mix (finalColor, color, fade);
  }

  // Set the fragment color
  finalColor = mix (pow (finalColor, vec3 (GAMMA)) + glowColor, vec3 (1.0), modeSwitch);
  fragColor = vec4 (finalColor, 1.0);
}