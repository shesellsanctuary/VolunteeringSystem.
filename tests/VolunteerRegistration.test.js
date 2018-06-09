const { expect } = require('chai');
const path = require('path');
const pg = require('pg');
const testUsers = require('./testUsers.json');
const { registerVolunteer, navigateToRegistationScreen } = require('./utils.js');

const dbConfig = { 
  user: 'vol_sys_postgres_db_admin',
  password: 'valpwd4242',
  database: 'orphanage',
  host: 'volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com',
  port: 5432
};

describe('Create a new volunteer',  function() {
    
  let page;
  this.timeout(0);

  before(async () => {
    page = await browser.newPage();
    await page.goto(TEST_ENV);
  });
    
  it('Should have main registration button and carousel registration buttons', async () => {
    const carouselSize = await page.evaluate(() => document.getElementById("myCarousel").children[0].children.length);
    const linkHandler = await page.$x("//a[contains(text(), 'Cadastre-se')]");

    expect(carouselSize + 1).to.equals(linkHandler.length);
  });

  it('Should navigate to registration screen when clicking on registration button', async () => {

    await navigateToRegistationScreen(page);

    expect(await page.url()).to.equals(`${TEST_ENV}Volunteer/Register`);
    expect(await page.title()).to.equals('Register - Voluntariado');
  });

  it('Should register one volunteer', async () => {
    await registerVolunteer(testUsers.volunteers[0], page);
    const warning = await page.$x("//span[contains(text(), 'Seu cadastro ainda não foi aprovado por nossos administradores')]");
    expect(warning.length).to.equals(1);
  });

  it('Should logout from main dashboard', async () => {
    const exitButton = await page.$x("//a[contains(text(), 'Sair')]");
    await exitButton[0].click();
    await page.waitForNavigation();
    expect(await page.url()).to.equals(TEST_ENV);

  });

  it('Should fail to register the same user', async () => {
    
    await navigateToRegistationScreen(page);    
    await registerVolunteer(testUsers.volunteers[0], page);  
    expect(await page.url()).to.equals(`${TEST_ENV}Volunteer/Register`);

    const alreadyExists = await page.$x("//b[contains(text(), 'Usuário já existe, por favor insira um e-mail não cadastrado')]");
    expect(alreadyExists.length).to.equals(1);
  });

  after (async () => {
    let client = new pg.Client(dbConfig);
    await client.connect();
    await client.query(`DELETE FROM volunteer WHERE email='${testUsers.volunteers[0].email}'`)
    await client.query(`DELETE FROM credential WHERE email='${testUsers.volunteers[0].email}'`);

    await client.end();
    await page.close();
  })
});