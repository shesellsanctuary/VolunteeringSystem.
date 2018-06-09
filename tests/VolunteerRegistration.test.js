const { expect } = require('chai');
const path = require('path');
const pg = require('pg');
const testUsers = require('./testUsers.json');
const { registerVolunteer, navigateToRegistationScreen, loginAsAdmin } = require('./utils.js');

const dbConfig = { 
  user: 'vol_sys_postgres_db_admin',
  password: 'valpwd4242',
  database: 'orphanage',
  host: 'volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com',
  port: 5432
};

describe('Create a new volunteer',  function() {
    
  let page;
  let client;
  this.timeout(0);

  before(async () => {
    client = new pg.Client(dbConfig);
    await client.connect();
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

  it('Should register second volunteer', async () => {
    await registerVolunteer(testUsers.volunteers[1], page);
    const warning = await page.$x("//span[contains(text(), 'Seu cadastro ainda não foi aprovado por nossos administradores')]");
    expect(warning.length).to.equals(1);
  });

  it('Should check if users were included in DB', async () => {
    const volunteer0 = await client.query(`SELECT * FROM volunteer WHERE email='${testUsers.volunteers[0].email}'`);
    const credential0 = await client.query(`SELECT * FROM credential WHERE email='${testUsers.volunteers[0].email}'`);
    const volunteer1 = await client.query(`SELECT * FROM volunteer WHERE email='${testUsers.volunteers[1].email}'`);
    const credential1 = await client.query(`SELECT * FROM credential WHERE email='${testUsers.volunteers[1].email}'`);
    const volunteer2 = await client.query(`SELECT * FROM volunteer WHERE email='${testUsers.volunteers[2].email}'`);
    const credential2 = await client.query(`SELECT * FROM credential WHERE email='${testUsers.volunteers[2].email}'`);

    expect(volunteer0.rowCount).to.equals(1);
    expect(credential0.rowCount).to.equals(1);
    expect(volunteer1.rowCount).to.equals(1);
    expect(credential1.rowCount).to.equals(1);
    expect(volunteer2.rowCount).to.equals(0);
    expect(credential2.rowCount).to.equals(0);
  });

  it('Should check if user is in approved tab (as a admin)', async () => {
    
    const exitButton = await page.$x("//a[contains(text(), 'Sair')]");
    await exitButton[0].click();
    await page.waitForNavigation();

    const adminArea = await page.$x("//a[contains(text(), 'Área do Admin')]");
    await adminArea[0].click();
    await page.waitForNavigation();
    expect(await page.url()).to.equals(`${TEST_ENV}Admin/Login`);

    await loginAsAdmin(page);
    await page.goto(`${TEST_ENV}Volunteer/List?status=0`);

    const pendingCPF = await page.$x(`//td[contains(text(), '${testUsers.volunteers[0].cpf}')]`);
    const pendingName = await page.$x(`//td[contains(text(), '${testUsers.volunteers[0].name}')]`);

    await page.waitFor(10000);
    
    expect(pendingCPF.length).to.equals(1);
    expect(pendingName.length).to.equals(1);
  }); 

  after (async () => {

    await client.query(`DELETE FROM volunteer WHERE email='${testUsers.volunteers[0].email}'`)
    await client.query(`DELETE FROM credential WHERE email='${testUsers.volunteers[0].email}'`);
    await client.query(`DELETE FROM volunteer WHERE email='${testUsers.volunteers[1].email}'`)
    await client.query(`DELETE FROM credential WHERE email='${testUsers.volunteers[1].email}'`);

    await client.end();
    await page.close();
  })
});