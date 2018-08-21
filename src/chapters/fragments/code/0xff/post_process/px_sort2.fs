precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform int u_frame;
uniform sampler2D u_t0;

void main( )
{
    vec2 uv = gl_FragCoord.xy / u_res.xy;
    vec2 texel = 1. / u_res.xy;

    float step_y = texel.y;
    vec2 s  = vec2(0.0, -step_y);
    vec2 n  = vec2(0.0, step_y);

    vec4 im_n =  texture2D(u_t0, uv+n);
    vec4 im =    texture2D(u_t0, uv);
    vec4 im_s =  texture2D(u_t0, uv+s);

    // use luminance for sorting
    float len_n = dot(im_n, vec4(0.299, 0.587, 0.114, 0.));
    float len = dot(im, vec4(0.299, 0.587, 0.114, 0.));
    float len_s = dot(im_s, vec4(0.299, 0.587, 0.114, 0.));

    if(int(mod(float(u_frame) + gl_FragCoord.y, 2.0)) == 0) {
        if ((len_s > len)) {
            im = im_s;
        }
    } else {
        if ((len_n < len)) {
            im = im_n;
        }
    }

    // blend with image
    if(u_frame<1) {
        gl_FragColor = texture2D(u_t0, uv);
    } else {
        gl_FragColor = (texture2D(u_t0, uv) + im * 99. ) / 100.;
    }
}