async function navigateToRegistationScreen(page) {

  const registrationButtons = await page.$x("//a[contains(text(), 'Cadastre-se')]");
  const registrationButton = registrationButtons[registrationButtons.length - 1];
  await registrationButton.click();
  await page.waitForNavigation();
}

async function registerVolunteer(volunteer, page) {
  

  await _clearAndType(page, '#inputName', volunteer.name);
  await _clearAndType(page, '#inputEmail', volunteer.email);

  await page.click('#inputBirthdate');
  await fillDateField(page, volunteer.birthdate);
  
  await _clearAndType(page, '#inputCPF', volunteer.cpf);

  await page.select('#inputSex', volunteer.sex);
  await _clearAndType(page, '#inputProfession', volunteer.profession);
  await _clearAndType(page, '#inputAddress', volunteer.address);
  await _clearAndType(page, '#inputPhone', volunteer.phone);
  
  const inputPhoto = await page.$('#inputPhoto');
  const photoPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.photo}`;
  await inputPhoto.uploadFile(photoPath);
  await page.waitFor(2000);
  
  const inputCriminalRecord = await page.$('#inputCriminalRecord');
  const criminalRecordPath = `C:\\git\\VolunteeringSystem\\VolunteeringSystem\\wwwroot\\assets\\${volunteer.criminalRecord}`;//path.relative(process.cwd(), __dirname + volunteer.criminalRecord);
  await inputCriminalRecord.uploadFile(criminalRecordPath);
  await page.waitFor(2000);
  
  await _clearAndType(page, '#inputPassword' ,volunteer.pwd);
  await _clearAndType(page, '#inputPasswordCheck' ,volunteer.pwd);

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

async function loginAsAdmin(page) {
  await page.click('#inputEmail');
  await page.keyboard.type('otavio@jacobi.com');
  await page.click('#inputPassword');
  await page.keyboard.type('yes');
  
  await page.keyboard.press('Enter');
  await page.waitForNavigation();
}

async function _clearAndType(page, selector, text) {
  const elementHandle = await page.$(selector);
  await elementHandle.click();
  await elementHandle.focus();
  await elementHandle.click({clickCount: 3});
  await elementHandle.press('Backspace');
  await elementHandle.type(text);
}

module.exports = {
  navigateToRegistationScreen,
  registerVolunteer,
  fillDateField,
  loginAsAdmin
}