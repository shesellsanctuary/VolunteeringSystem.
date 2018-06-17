const { expect } = require('chai');
const path = require('path');
const pg = require('pg');
const testUsers = require('./testUsers.json');
const { loginAsAdmin, loginAsVolunteer, clearAndType, fillDateField, logout } = require('./utils.js');

const dbConfig = { 
  user: 'vol_sys_postgres_db_admin',
  password: 'valpwd4242',
  database: 'orphanage',
  host: 'volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com',
  port: 5432
};

describe('Create a new event',  function() {
    
  let page;
  let client;
  this.timeout(0);

  before(async () => {
    client = new pg.Client(dbConfig);
    await client.connect();
    page = await browser.newPage();
    await page.goto(`${TEST_ENV}Volunteer/Login`);
    await loginAsVolunteer(page);
  });

  it('Should go to event creation screen', async () => {
    const createEventButton = await page.$x("//a[contains(text(), 'Criar evento')]");
    await createEventButton[0].click();
    await page.waitForNavigation();

    expect(await page.url()).to.equals(`${TEST_ENV}Event`);
  });
  
  it('Should try to create event with invalid number of children and fail', async () =>  {
    await fillEventForm(page, {
      ageGroup: '1',
      kidLimit: '6',
      date: '12/21/2018',
      description: 'A test description'
    });

    const submitEventButton = await page.$x("//button[contains(text(), 'Submeter Evento')]");
    await submitEventButton[0].click();
    await page.waitFor(2000);

    expect(await page.url()).to.equals(`${TEST_ENV}Event`);
  });

  const correctEventParameters = {
    ageGroup: '2',
    kidLimit: '3',
    date: '11/21/2018',
    description: 'A test description'
  };

  it('Should create new event', async () =>  {
    await fillEventForm(page, correctEventParameters);

    const submitEventButton = await page.$x("//button[contains(text(), 'Submeter Evento')]");
    await submitEventButton[0].click();
    await page.waitForNavigation();

    expect(await page.url()).to.equals(`${TEST_ENV}Event/Created`);

    await page.waitFor(3000);
  });

  it('Should check if event was created in DB', async () => {
    const event = await client.query(`SELECT * FROM event WHERE description='${correctEventParameters.description}'`);
    expect(event.rows[0].age_group_id).to.equals(+correctEventParameters.ageGroup);
    expect(event.rows[0].kid_limit).to.equals(+correctEventParameters.kidLimit);
  });

  it('Should logout', async () => {
    await logout(page);
    expect(await page.url()).to.equals(TEST_ENV);
  });
  
  it('Should try to create an event with a not yet approved account', async () => {
    await page.goto(`${TEST_ENV}Volunteer/Login`);
    await loginAsVolunteer(page, {user: 'pietra@castro.com', password: 'no'});
    const createEventButton = await page.$x("//a[contains(text(), 'Criar evento')]");
    await createEventButton[0].click();
    await page.waitForNavigation();

    expect(await page.url()).to.equals(`${TEST_ENV}Event/Denied`);
  });

  it('Should try to create an event with a blocked account', async () => {
    await page.goto(`${TEST_ENV}Volunteer/Login`);
    await loginAsVolunteer(page, {user: 'pedro@cardoso.com', password: 'no'});
    const createEventButton = await page.$x("//a[contains(text(), 'Criar evento')]");
    await createEventButton[0].click();
    await page.waitForNavigation();

    expect(await page.url()).to.equals(`${TEST_ENV}Event/Denied`);
  });

  after (async () => {

    await client.query(`DELETE FROM event WHERE description='A test description'`);

    await client.end();
    await page.close();
  })
});

async function fillEventForm(page, values) {
  await page.select('#ageGroup_label', values.ageGroup);
  await clearAndType(page, '#kidLimit', values.kidLimit);
  await page.click('#date');
  await fillDateField(page, values.date);
  await clearAndType(page, '#description', values.description);  
}