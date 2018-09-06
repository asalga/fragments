/*
    Andor Saga
*/
'use strict';

let relPath = '../../chapters/fragments/code/0xff/';

let imgTest;

// note the last xy tuple is a copy of the second last so that
// all 3 shapes have the same length which makes the shader have less code
let type0 = [-0.25, -1, -1, -0.25, -0.5, -0.06, -1, 0.25, 
            -0.47, 0.88, 0.03, 0.25, 0.03, 0.88, 0.56, 0.88, 
            1, 0.25, 1, -0.25, 0.5, -1, -0.25, -1, 
            -0.25, -1];// degenerate line here

// len: 26
let type1 = [-0.4, -1, -0.21, -0.51, -1, -0.51, -1, 0.21, 
            -0.45, 0.81, 0.21, 0.63, 0.512, 0.89, 1, 0.39, 
            0.22, 0, 1, -0.30, 1, -0.5, 0.5, -1, 
            -0.4, -1];

// len: 26
let type2 = [ -0.45, -1, -1, -0.51, -0.69, -0.08, -1, 0.42,
              -0.45, 0.85, -0.2, 0.63, 0.54, 0.85, 1, 0.17,
              0.54, -0.32, 1, -0.51, 0.54, -1, 0.02, -0.75,
              -0.45, -1];

let demo = {
  'size': {
    'width': 512,
    'height': 512
  },
  '0': {
    // src: '100_199/0/105_cube_walk.fs',
    // src: '100_199/0/108_seed_of_life.fs',
    // src: '100_199/2/120_box_tex_displace.fs',
    // src: '100_199/3/130_little_critters.fs',
    // src: '100_199/3/132_vel.fs',
    // src: '100_199/3/133_helpful_mushroom.fs',
    // src: '100_199/3/135_platforms_and_palettes.fs',
    // src: '100_199/3/136_rollers.fs',
    // src: '100_199/2/129_invading_space.fs',
    
    // src: '100_199/3/139_blocky_line.fs',
    // src: '100_199/3/139_endless_struggle.fs',
    // src: '100_199/4/140_vx.fs',
    // src: '100_199/4/142_primitives.fs',
    // src: '100_199/4/143_windows.fs',
    // src: '100_199/4/144_blocky.fs',
    // src: '100_199/4/14x_brick_tunnel.fs',
    // src: '100_199/4/145.fs',
    // src: '100_199/4/147_dual.fs',
    // src: '100_199/4/148_dreamcatcher.fs',
    // src: '100_199/4/149_diaid.fs',

    // src: '100_199/4/14x_voxel_tutorial2.fs',

    // src: '100_199/4/glow1.fs',
    // src: '100_199/4/146_flow.fs',
    // src: '100_199/4/retro_parallax.fs',
    // src: '100_199/4/_.fs',
    // src: '100_199/4/143_fly_casual.fs',
    // src: '0_99/80/80_world_0_0.fs',

    // src: '100_199/5/14x_windows.fs',

    // 5
    // src: '100_199/5/150_wax_and_wane.fs',
    // src: '100_199/5/151_slash-etc.fs',
    // src: '100_199/5/152_alt_tunnel.fs',
    // src: '100_199/5/153_mixing.fs',
    // src: '100_199/5/154_infinite.fs',
    // src: '100_199/5/155_brick_building.fs',
    
    // src: '100_199/5/156_triad.fs',
    // src: '100_199/5/157_over.fs',
    // src: '100_199/5/158_connexions.fs',


    // 6
    src: '100_199/6/161_diaid_2.fs',
    
    
    // src: '100_199/5/15x_all_platforms_levitate.fs',

    uniforms: [
      // {'name': 'u_fov', 'value': 70}
      {'name': 'u_type0', 'value': type0},
      {'name': 'u_type1', 'value': type1},
      {'name': 'u_type2', 'value': type2}
    ]
  },
  '1': {
    src: 'post_process/null.fs',
    // src: 'post_process/simple_dither.fs',
    // src: 'post_process/182.fs',
    // src: 'post_process/182_voronoi.fs',
    // src: 'post_process/px_sort2.fs',
    // src: 'post_process/blur.fs',
    // src: 'post_process/static.fs',
    // src: 'post_process/flood.fs',
    // src: 'post_process/cel.fs',
    // src: 'post_process/pixelate.fs',
    // src: 'post_process/tunnel.fs',
    // src: 'post_process/sobel.fs',
    // src: 'post_process/parallax.fs',

    uniforms: [
      { 'name': 'u_numShades', 'value': 12 },
      {
        'name': 'u_pixelSize',
        'value': function(s) {
          return 5.;
          return Math.sin(s / 2.) * 10;
        }
      },
      { 'name': '_', 'value': [-1, -1, 0, -1, 1, -1, -1, 0, 0, 0, 1, 0, -1, 1, 0, 1, 1, 1] }
    ]
  }
};


let globalVs = `
precision highp float;
attribute vec3 aPosition;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
}`;


let gif, YesMakeGif = false;

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  // TWEEN.update(time);
}
requestAnimationFrame(animate);


/*
  fs - array of frag shaders
*/
function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;

  let w, h;
  let img0, img1, img2;

  // let timeVal = { t: 0 };
  let sketchTime;
  let epochTime;
  let tracking = [];
  let easing = 0.05;
  let start = 0;
  let mouseIsDown = 0;
  let lastMouseDown = [120, 200];
  // let shader_0_Frag, shader_1_Frag;

  let gfx, shader_0, shader_1;
  let shader_0_Frag = fs[0];
  let shader_1_Frag = fs[1];
  let ditherTex;
  let ditherMat = [
    7, 9, 5,
    2, 1, 4,
    6, 3, 8
  ];

  var sketch = function(p, relPath) {



    p.preload = function() {
      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, vec2(0.5));
                      gl_FragColor = vec4(col.rg,1, 1);
                    }`;
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];

      ditherTex = p.createGraphics(3, 3, p.P2D);
      ditherTex.pixelDensity(1);
      ditherTex.canvas.style = ''; // remove the 'display:none' to see the cvs

      // Dither texture/matrix
      // let ditherMat = [
      // 2,5,3,4,1,6,7,8
        // 7, 9, 5,
        // 2, 1, 4,
        // 6, 3, 8
      // ];

      for (let x = 0; x < 3; ++x) {
        for (let y = 0; y < 3; ++y) {
          let col = (ditherMat[y * 3 + x] / 10) * 255;
          ditherTex.stroke(col);
          ditherTex.point(x, y);
        }
      }

      p.createCanvas(w, h, p.WEBGL);
      gfx = p.createGraphics(w, h, p.WEBGL);
      gfx.pixelDensity(1);

      shader_0 = new p5.Shader(gfx._renderer, globalVs, shader_0_Frag);
      shader_1 = new p5.Shader(p._renderer, globalVs, shader_1_Frag);

      p.pixelDensity(1);
      $(p.canvas).appendTo($('#target'));
      $(ditherTex.canvas).appendTo($('#target2'));
      // p.noLoop();

      imgTest = p.loadImage('../chapters/fragments/tex/tex.jpg');
    };

    p.mouseClicked = function() {
      start = p.millis();
    };

    /**
      Draw
    */
    p.draw = function() {
      let [width, height] = [w, h];
      let sz = 1.;
      sketchTime = (p.millis() - start) / 1000;
      epochTime = Math.floor(new Date().getTime()/10000);

      // totally mess with the dithering :)
      // if( p.frameCount % 40 == 0){
      //   for(let i = 0; i < 30; i++){
      //     let _one = Math.floor(Math.random()*9);
      //     let _two = Math.floor(Math.random()*9);
      //     [ditherMat[_one], ditherMat[_two]] = [ditherMat[_two], ditherMat[_one]] ;
      //   }
      // }

      // // Create our dither texture
      // for (let x = 0; x < 3; ++x) {
      //   for (let y = 0; y < 3; ++y) {
      //     let col = (ditherMat[y * 3 + x] / 10) * 255;
      //     ditherTex.stroke(col);
      //     ditherTex.point(x, y);
      //   }
      // }

      gfx.push();
      gfx.translate(width / 2, height / 2);
      gfx.shader(shader_0);


      shader_0.setUniform('u_res', [width, height]);
      shader_0.setUniform('u_mouse', [p.mouseX.clamp(0, w) / width, p.mouseY.clamp(0, h) / height, mouseIsDown]);
      shader_0.setUniform('u_time', sketchTime);
      shader_0.setUniform('u_epochTime', epochTime);
      shader_0.setUniform('u_t0', imgTest);
      
      
      // if(frameCount < 10){
        // shader_0.setUniform('u_t0', gfx);
        // after rendering,
         // get pixels
         // instead o
      // }

      shader_0.setUniform('u_frame', p.frameCount);
      demo[0].uniforms.forEach(v => { // custom uniforms
        shader_0.setUniform(v.name, v.value);
      });

      gfx.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      gfx.pop();




      // Post Processing
      p.push();
      p.translate(width / 2, height / 2);
      p.shader(shader_1);
      shader_1.setUniform('u_res', [width, height]);
      shader_1.setUniform('u_time', sketchTime);
      shader_1.setUniform('u_t0', gfx);
      shader_1.setUniform('u_frame', p.frameCount);
      // shader_1.setUniform('u_pixelSize', 1.+Math.floor(p.mouseX/10) );
      shader_1.setUniform('u_ditherTex', ditherTex);

      demo[1].uniforms.forEach(v => { // custom uniforms
        let val = typeof v.value === 'function' ? v.value(sketchTime) : v.value;
        shader_1.setUniform(v.name, val);
      });

      p.rect(-width * sz, -height * sz, width * sz, height * sz, 2, 2);
      p.pop();

    }; //end draw
  };
  return sketch;
}



function getFs0() {
  return fetch(relPath + demo[0].src)
    .then(res => res.text())
}

function getFs1() {
  return fetch(relPath + demo[1].src)
    .then(res => res.text())
}


(function load() {
  Promise.all([getFs0(), getFs1()])
    .then(fragShaders => {
      
      let sketch = new p5(makeSketch(fragShaders, demo.size), relPath);
    });
})();