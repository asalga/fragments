precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

void main( ){
    vec2 p =  (gl_FragCoord.xy / u_res)*2.-1.;
    float len = length(p);
    float t = u_time * 10.;

    float rings1 = cos(pow(len, 2.0) * p.x * 60. + t);
    float rings2 = sin(pow(len, 2.0) * p.y * 60. + t);

    float i =  rings1 * rings2;

    gl_FragColor = vec4(vec3(i), 1);
}
