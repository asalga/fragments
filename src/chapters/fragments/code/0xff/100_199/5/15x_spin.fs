
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 p = (-iResolution.xy + 2.0*fragCoord)/iResolution.x;

    float r2 = dot(p,p);
    float r = sqrt(r2);


        vec2 uv = p/r2;


    // animate
	uv += 10.0*cos( vec2(0.6,0.3) + vec2(0.1,0.13)*iTime );

	vec3 col = r * texture( iChannel0,uv*.25).xyz;

    fragColor = vec4( col, 1.0 );
}