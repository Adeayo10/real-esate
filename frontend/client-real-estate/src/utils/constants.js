import { decodeAccessToken } from './token';
import { getAccessToken } from './token';


export const TOAST_OPTIONS = {
  position: "top-right",
  autoClose: 3000,
  hideProgressBar: true,
  closeOnClick: true,
  pauseOnHover: false,
  draggable: false,
  progress: undefined,
  theme: "light",
};

export const storeUserIdFromToken = () => {
  const accessToken = getAccessToken(); 
  if (!accessToken) {
    console.warn('Access token not found in localStorage.');
    return;
  }

  const decodedToken = decodeAccessToken(accessToken);
  if (decodedToken && decodedToken.sub) {
    localStorage.setItem('userId', decodedToken.sub);
    console.log('User ID stored in localStorage:', decodedToken.sub);
    return decodedToken.sub; 
    
  } else {
    console.error('Failed to decode access token or extract user ID.');
  }
};
const  ACCOUNT_SID = "AC12ffa38f3b4d498cc3d8fc89f8515890";
const AUTH_TOKEN = "a618f49cc2ec6d2e2ee63c19c83e5c9d";
export const sendSMS = async (to, message) => {
  const url = 'https://api.twilio.com/2010-04-01/Accounts/AC12ffa38f3b4d498cc3d8fc89f8515890/Messages.json';
  const auth = btoa(`${ACCOUNT_SID}:${AUTH_TOKEN}`);
  
  try {
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Authorization': `Basic ${auth}`,
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        To: to,
        From: '+14238301785',
        Body: message,
      }),
    });

    if (!response.ok) {
      throw new Error(`Failed to send SMS: ${response.statusText}`);
    }

    const data = await response.json();
    console.log('SMS sent successfully:', data);
    return data;
  } catch (error) {
    console.error('Error sending SMS:', error);
    throw error;
  }
};

export const formatPhoneNumber = (phoneNumber) => {
  if (phoneNumber.startsWith("0")) {
    return "+353" + phoneNumber.slice(1); 
  } 
}
