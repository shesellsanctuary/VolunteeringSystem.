const { expect } = require('chai');
const path = require('path');
const pg = require('pg');
const testUsers = require('./testUsers.json');
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
    const registrationButtons = await page.$x("//a[contains(text(), 'Cadastre-se')]");
    const registrationButton = registrationButtons[registrationButtons.length - 1];

    await registrationButton.click();
    await page.waitForNavigation();

    expect(await page.url()).to.equals(`${TEST_ENV}Volunteer/Register`);
    expect(await page.title()).to.equals('Register - Voluntariado');
  });

  it('Should register one volunteer', async () => {
    await registerVolunteer(testUsers.volunteers[0], page);
    const warning = await page.$x("//span[contains(text(), 'Seu cadastro ainda não foi aprovado por nossos administradores')]");
    expect(warning.length).to.equals(1);
  });

  after (async () => {
    let client = new pg.Client(dbConfig);
    await client.connect();
    await client.query(`DELETE FROM volunteer WHERE email='${testUsers.volunteers[0].email}'`)
    await client.query(`DELETE FROM credential WHERE email='${testUsers.volunteers[0].email}'`);


    client.end();
    await page.close();
  })
});

async function registerVolunteer(volunteer, page) {

  await page.click('#inputName');
  await page.keyboard.type(volunteer.name);
  await page.click('#inputEmail');
  await page.keyboard.type(volunteer.email);
  await page.click('#inputBirthdate');
  await fillDateField(page, volunteer.birthdate);
  await page.click('#inputCPF');
  await page.keyboard.type(volunteer.cpf);
  await page.select('#inputSex', volunteer.sex);
  await page.click('#inputProfession');
  await page.keyboard.type(volunteer.profession);
  await page.click('#inputAddress');
  await page.keyboard.type(volunteer.address);
  await page.click('#inputPhone');
  await page.keyboard.type(volunteer.phone);

  const inputPhoto = await page.$('#inputPhoto');
  const photoPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.photo}`;
  await inputPhoto.uploadFile(photoPath);
  await page.waitFor(2000);

  const inputCriminalRecord = await page.$('#inputCriminalRecord');
  const criminalRecordPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.criminalRecord}`;//path.relative(process.cwd(), __dirname + volunteer.criminalRecord);
  await inputCriminalRecord.uploadFile(criminalRecordPath);
  await page.waitFor(2000);

  await page.click('#inputPassword');
  await page.keyboard.type(volunteer.pwd);
  await page.click('#inputPasswordCheck');
  await page.keyboard.type(volunteer.pwd);

  await page.keyboard.press('Enter');
  await page.waitForNavigation();
  await page.waitFor(5000);
  
}

async function fillDateField(page, date) {
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[0]);
  await page.waitFor(2);
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[1]);
  await page.waitFor(2);
  await page.keyboard.press('Backspace');
  await page.keyboard.type(date.split('/')[2]);
}