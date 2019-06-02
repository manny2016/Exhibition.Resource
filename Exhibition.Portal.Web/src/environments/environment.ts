// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  api:"https://localhost:44378/api/mgr/"
};

export const directiveTypes=[
  {key: 1,text:"开机"},
  {key: 2,text:"关机"},
  {key: 3,text:"播放视频"},
  {key: 4,text:"播放图片集"},
  {key: 5,text:"切换播放模式"},
  {key: 6,text:"重新启动"},
  {key: 7,text:"关闭监视器"},
  {key: 8,text:"停止当前播放任务"},
]