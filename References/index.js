const Storage = require('@google-cloud/storage');
const Dialogflow = require('dialogflow');

// Instantiates a client. Explicitly use service account credentials by
// specifying the private key file. All clients in google-cloud-node have this
// helper, see https://github.com/GoogleCloudPlatform/google-cloud-node/blob/master/docs/authentication.md

/* Storage API 테스트 코드

const storageAPI = new Storage({
  keyFilename: 'D:\\Programs\\Google\\Small-Talk-ab19e1b0105f.json'
});

// Makes an authenticated API request.
storageAPI
  .getBuckets()
  .then((results) => {
    const buckets = results[0];

    console.log('Buckets:');
    buckets.forEach((bucket) => {
      console.log(bucket.name);
    });
  })
  .catch((err) => {
    console.error('ERROR:', err);
  });
  //*/

  // You can find your project ID in your Dialogflow agent settings
const projectId = 'small-talk-7a50b'; //https://dialogflow.com/docs/agents#settings
const sessionId = 'quickstart-session-id';
const query = '안녕?';
const languageCode = 'ko-KR';

// Instantiate a DialogFlow client.
/** 원래는 keyFile을 명시할 필요 없이 GOOGLE_APPLICATION_CREDENTIALS 변수에 경로를 넣으면 
 * Default에 설정이 되서 자동으로 authentication이 이루이 짐
 * 그러나 콘솔 창에서 설정을 해도 해당 변수가 유지되지 않음, 변수를 유지하기 위한 방안을 알아봐야 함
 * 참고 --->
 * With PowerShell:
 * 
 * $env:GOOGLE_APPLICATION_CREDENTIALS="[PATH]"
 * For example:
 * $env:GOOGLE_APPLICATION_CREDENTIALS="C:\Users\username\Downloads\[FILE_NAME].json"
 * 
 * With command prompt:
 * 
 * set GOOGLE_APPLICATION_CREDENTIALS=[PATH]
*/
const sessionClient = new Dialogflow.SessionsClient({
  keyFilename: 'D:\\Programs\\Google\\Small-Talk-ab19e1b0105f.json'
});

// Define session path
const sessionPath = sessionClient.sessionPath(projectId, sessionId);

// The text query request.
const request = {
  session: sessionPath,
  queryInput: {
    text: {
      text: query,
      languageCode: languageCode,
    },
  },
};

// Send request and log result
sessionClient.detectIntent(request).then(responses => {
  console.log('Detected intent');
  const result = responses[0].queryResult;
  console.log(`  Query: ${result.queryText}`);
  console.log(`  Response: ${result.fulfillmentText}`);
  if (result.intent) {
    console.log(`  Intent: ${result.intent.displayName}`);
  } else {
    console.log(`  No intent matched.`);
  }
}).catch(err => {
  console.error('ERROR:', err);
});