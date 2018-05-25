const { expect } = require('chai');

describe('Create a new volunteer',  function() {
    
    let page;
    this.timeout(5000);

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
        const registrationButton = (await page.$x("//a[contains(text(), 'Cadastre-se')]"))[0];
        
        await registrationButton.click();
        await page.waitForNavigation();

        expect(await page.url()).to.equals(`${TEST_ENV}Volunteer/Register`);
        expect(await page.title()).to.equals('Register - VolunteeringSystem');
    });

    it('Should register one user', async () => {
        await registerTestVolunteer();
    });


    after (async () => {
        await page.close();
    });
  });