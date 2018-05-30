#ifdef GL_ES
precision highp float;
#endif

const float PI = 3.14159265358979323846264;
const float TWOPI = PI*2.0;

const vec4 WHITE = vec4(1.0, 1.0, 1.0, 1.0);
const vec4 BLACK = vec4(0.0, 0.0, 0.0, 1.0);

const vec2 CENTER = vec2(0.0, 0.0);

const int MAX_RINGS = 40;
const float RING_DISTANCE = 0.05;
const float WAVE_COUNT = 0.0;
const float WAVE_DEPTH = 0.04;

uniform float u_time;
uniform vec2 u_res;

void main(void) {
    vec2 a = vec2(1., u_res.y/u_res.x);
    vec2 p = a*vec2(gl_FragCoord.xy/u_res) *2. -1.;
    float x = p.x;
    float y = p.y;
    

    bool black = false;
    float prevRingDist = RING_DISTANCE;
    for (int i = 0; i < MAX_RINGS; i++) {
        vec2 center = vec2(0.0, 1.3 - RING_DISTANCE * float(i)*1.2);

        float radius = 0.5 + RING_DISTANCE / (pow(float(i+5), 1.1)*0.006);
        float dist = distance(center, p );
        dist = pow(dist, 0.5);
        
        float ringDist = abs(dist-radius);
        if (ringDist < RING_DISTANCE*prevRingDist*7.0) {
            float angle = atan(y - center.y, x - center.x);
            float thickness = 1.1 * abs(dist - radius) / prevRingDist;
            
            float depthFactor = 0.;//WAVE_DEPTH * sin((angle+rot*radius) * WAVE_COUNT);

            if (dist > radius) {
                black = (thickness < RING_DISTANCE * 5.0 - depthFactor * 2.0);
            }
            else {
                black = (thickness < RING_DISTANCE * 5.0 + depthFactor);
            }
            break;
        }
        if (dist > radius) break;
        prevRingDist = ringDist;
    }

    gl_FragColor = black ? BLACK : WHITE;
}