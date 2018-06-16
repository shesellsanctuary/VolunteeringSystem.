const puppeteer = require('puppeteer');
const { expect } = require('chai');
const _ = require('lodash');
const globalVariables = _.pick(global, ['browser', 'expect', 'TEST_ENV']);

// puppeteer options
const opts = {
  headless: false,
  slowMo: 30,
  timeout: 30000
};

before (async () => {
  global.expect = expect;
  global.TEST_ENV = 'http://localhost:55556/';
  global.browser = await puppeteer.launch(opts);
});

after (() => {

  //TODO: delete all used users, voluneers, events from db....
  browser.close();

  global.browser = globalVariables.browser;
  global.expect = globalVariables.expect;
});