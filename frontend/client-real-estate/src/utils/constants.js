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
    return decodedToken.sub; // Return the user ID for further use if needed
    
  } else {
    console.error('Failed to decode access token or extract user ID.');
  }
};
