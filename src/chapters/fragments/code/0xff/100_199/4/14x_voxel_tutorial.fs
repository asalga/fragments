precision mediump float;

uniform vec2 u_res;
uniform vec3 u_mouse;
uniform float u_time;

#define VOXEL_RESOLUTION  3.
#define VOXEL_LIGHTING

#define CAMERA_FOCAL_LENGTH 2.0
#define DELTA       0.00001
#define RAY_LENGTH_MAX    500.0
#define RAY_STEP_MAX    200.0
#define AMBIENT       0.2

#define PI    3.14159265359
#define SQRT3 1.73205080757

float sdBox(in vec3 p) {
  vec3 sz = vec3(1.);
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));
  return insideDistance + outsideDistance;
}

vec2 distScene (in vec3 p, out vec3 P) {
  p *= VOXEL_RESOLUTION;
  float body = length (p) - (sin(u_time)+1.)/2. * 40.;
  vec2 d = vec2 (body);
  d /= VOXEL_RESOLUTION;
  return d;
}

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
      d.x = sdBox (fract (p + 0.5) - 0.5);
      if (d.x < DELTA) {
        break;
      }
    }
    else if (rayLength < rayLengthCheckVoxel) {
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
    }
     else {
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

  return vec4 (d, rayLength, 1.);
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}


mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

vec3 normal (in vec3 p, in float voxelized) {
  vec2 h = vec2 (DELTA, -DELTA);
  vec3 n;
  p = fract (p + 0.5) - 0.5;
  n = h.xxx * sdBox (p + h.xxx) +
      h.xyy * sdBox (p + h.xyy) +
      h.yxy * sdBox (p + h.yxy) +
      h.yyx * sdBox (p + h.yyx);

  return normalize (n);
}

void main () {
  float t = u_time;
  vec2 frag = (2.0 * gl_FragCoord.xy - u_res.xy) / u_res.y;

  float mode3D = 0.5;
  float modeVoxel = (1.0);

  float dd =110.;
  vec3 eye = vec3(dd * cos(t), 3.  , dd * sin(t));
  vec3 center = vec3(0, 0, 0.);

  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(100., u_res, gl_FragCoord.xy);
  vec3 worldDir = viewWorld * ray;

  // Compute the origin of the ray
  float cameraDist = 110.;// mix (300.0, 95.0 + 50.0 * cos (u_time * 0.8), mode3D);
  vec3 origin = vec3(10., 10., 40.);

  // Compute the distance to the scene
  vec4 d = dist (origin, ray, modeVoxel, RAY_LENGTH_MAX / VOXEL_RESOLUTION);

  vec3 finalColor;
  if (d.x < DELTA) {
    vec3 color = vec3( modeVoxel);
    vec3 l =  vec3(0., 0., 1.);
    l = normalize(l);

    vec3 n = normal (origin, modeVoxel);
    float diffuse = dot (n, l);

    if(diffuse > 1.){
      diffuse = 1.;
    }
    // diffuse = max(diffuse, 0.);
    color = (AMBIENT + diffuse) * color;
    finalColor = mix (finalColor, color, 1.0);
  }

  gl_FragColor = vec4 (finalColor, 1.0);
}