async function navigateToRegistationScreen(page) {

  const registrationButtons = await page.$x("//a[contains(text(), 'Cadastre-se')]");
  const registrationButton = registrationButtons[registrationButtons.length - 1];
  await registrationButton.click();
  await page.waitForNavigation();
}

async function registerVolunteer(volunteer, page) {
  
  await clearAndType(page, '#inputName', volunteer.name);
  await clearAndType(page, '#inputEmail', volunteer.email);

  await page.click('#inputBirthdate');
  await fillDateField(page, volunteer.birthdate);
  
  await clearAndType(page, '#inputCPF', volunteer.cpf);

  await page.select('#inputSex', volunteer.sex);
  await clearAndType(page, '#inputProfession', volunteer.profession);
  await clearAndType(page, '#inputAddress', volunteer.address);
  await clearAndType(page, '#inputPhone', volunteer.phone);
  
  const inputPhoto = await page.$('#inputPhoto');
  const photoPath = `.\\assets\\${volunteer.photo}`;
  await inputPhoto.uploadFile(photoPath);
  await page.waitFor(2000);
  
  const inputCriminalRecord = await page.$('#inputCriminalRecord');
  const criminalRecordPath = `.\\assets\\${volunteer.criminalRecord}`;//path.relative(process.cwd(), __dirname + volunteer.criminalRecord);
  await inputCriminalRecord.uploadFile(criminalRecordPath);
  await page.waitFor(2000);
  
  await clearAndType(page, '#inputPassword' ,volunteer.pwd);
  await clearAndType(page, '#inputPasswordCheck' ,volunteer.pwd);

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

async function _login(page, credentials) {
  await page.click('#inputEmail');
  await page.keyboard.type(credentials.user);
  await page.click('#inputPassword');
  await page.keyboard.type(credentials.password);

  await page.keyboard.press('Enter');
  await page.waitForNavigation();
}

async function loginAsAdmin(page) {
  await _login(page, {user:'otavio@jacobi.com', password: 'yes'});
}

async function loginAsVolunteer(page, customUser) {
  await _login(page, customUser || {user:'test@volunteer.com', password: 'test'});
}

async function clearAndType(page, selector, text) {
  const elementHandle = await page.$(selector);
  await elementHandle.click();
  await elementHandle.focus();
  await elementHandle.click({clickCount: 3});
  await elementHandle.press('Backspace');
  await elementHandle.type(text);
}

async function logout(page) {
  const exitButton = await page.$x("//a[contains(text(), 'Sair')]");
  await exitButton[0].click();
  await page.waitForNavigation();
}

module.exports = {
  navigateToRegistationScreen,
  registerVolunteer,
  fillDateField,
  loginAsAdmin,
  loginAsVolunteer,
  clearAndType,
  logout
}