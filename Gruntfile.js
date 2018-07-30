'use strict';

module.exports = function(grunt) {
  const Livereload = 35729;
  const fs = require('fs');
  const ServeStatic = require('serve-static');

  const dst = 'dst';
  const src = 'src';
  const tmp = '.tmp';
  const app = 'app';

  require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);

  grunt.initConfig({

    /**
     * https://github.com/gruntjs/grunt-contrib-jshint
     * options inside .jshintrc file
     */
    jshint: {
      options: {
        jshintrc: '.jshintrc'
      },

      files: [
        `${src}/js/**/*.js`,
        // `${src}/json/**/*.js`,
        `!${src}/js/vendor/**/*.js`
      ]
    },

    // /*
    //  */
    // pug: {
    //   dev: {
    //     options: {
    //       pretty: true
    //     },
    //     files: [{
    //       src: `${src}/chapters/fragments/index.pug`,
    //       dest: `${tmp}/templates-compiled/index.html`
    //     }]
    //   }
    // },

    /** 
     */
    clean: {
      build: {
        src: [
          `${app}`,
          `${dst}`,
          `${tmp}`
        ]
      }
    },

    /**
     *
     */
    copy: {
      dev: {
        files: [
        // {
        //   expand: true,
        //   cwd: `${src}/js/`,
        //   src: ['**'],
        //   dest: `${app}/`,
        //   filter: 'isFile'
        // },
        {
          expand: true,
          cwd: `${src}/js`,
          src: ['main.js'],
          dest: `${app}/js`,
          filter: 'isFile'
        }
        ]
      },
      demos: {
        files: [{
          expand: true,
          cwd: `${src}/chapters/fragments/code/0xff/100_199/2`,
          // cwd: `${src}/chapters/fragments/code/0xff/90/`,
          // cwd: `${src}/chapters/fragments/code/0xff/_wip/`,
          // cwd: `${src}/chapters/fragments/code/ex/`,
          // cwd: `${src}/chapters/fragments/code/0xff/_wip/raymarch/`,
          src: ['**'],
          dest: `${app}/chapters/fragments/code/0xff/100_199/2`,
          // dest: `${app}/chapters/fragments/code/0xff/90/`,
          // dest: `${app}/chapters/fragments/code/ex/`,
          // dest: `${app}/chapters/fragments/code/0xff/_wip/`,
          // dest: `${app}/chapters/fragments/code/0xff/_wip/raymarch/`,
          filter: 'isFile'
        }]
      }
    },

    /**
     *  https://www.npmjs.com/package/grunt-processhtml
     *  process <!-- build:include --> directives
     */
    // processhtml: {
    //   dev: {
    //     options: {
    //       process: true
    //       // data: config,
    //       // strip: true,
    //     },
    //     files: [{
    //       'src': `${app}/chapters/fragments/index.html`,
    //       'dest': `${app}/chapters/fragments/index.html`
    //     }]
    //   }
    // },

    /**
     * Connect port/livereload
     * https://github.com/gruntjs/grunt-contrib-connect
     */
    connect: {
      options: {
        port: 9000,
        hostname: '*'
      },
      livereload: {
        options: {
          middleware: function(connect, options) {
            return [
              ServeStatic(`${app}`),
              connect().use(`${app}`, ServeStatic(`${app}`)),
              ServeStatic(`${app}`)
            ]
          }
        }
      }
    },

    /**
     * https://github.com/gruntjs/grunt-contrib-watch
     */
    watch: {
      options: {
        spawn: true,
        livereload: true
      },
      all: {
        files: [
          `${src}/js/**/*.*`,
          `${src}/chapters/fragments/code/0xff/100_199/1/**/*.*`,
          `${src}/chapters/fragments/code/0xff/100_199/2/**/*.*`
        ],
        tasks: [
          'copy:dev',
          'copy:demos',
          // 'pug',
          // 'processhtml'
        ],
        options: {
          livereload: true
        }
      }
    }
  });

  grunt.registerTask('default', [
    'copy',
    'connect:livereload',
    'watch'
  ]);
};