import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {
  getAccessToken,
  getRefreshToken,
  isTokenValid,
  setAccessToken,
} from "../utils/token"; // Assuming these functions are defined in a separate file

const API_URL = "/api/auth";

// Helper function to handle API requests
const apiRequest = async (endpoint, method, body = null, useAuth = false) => {
  const headers = {
    "Content-Type": "application/json",
  };

  if (useAuth) {
    let accessToken = getAccessToken();
    if (!accessToken) {
      console.warn('Access token not found in localStorage.');
      return null;
    }

    if (!isTokenValid(accessToken)) {
      console.warn(
        "Access token is invalid or expired. Attempting to refresh token."
      );

      try {
        const refreshTokenValue = getRefreshToken();

        if (!refreshTokenValue) {
          console.error("Refresh token not found. Logging out user.");
          logoutUser();
          return;
        }

        // Refresh the token
        accessToken = await renewAuthToken({
          accessToken,
          refreshToken: refreshTokenValue,
        });
        setAccessToken(accessToken); // Save the new token
      } catch (error) {
        console.error("Token refresh failed. Logging out locally.", error);
        logoutUser();
        return;
      }
    }

    // Add the Authorization header
    headers["Authorization"] = `Bearer ${accessToken}`;
  }

  try {
    const response = await fetch(`${API_URL}${endpoint}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : null,
    });

    if (!response.ok) {
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("API request error:", error);
    throw error.message;
  }
};

// Register a new user
export const registerUser = async (userData) => {
  return await apiRequest("/register", "POST", userData);
};

// Login a user
export const loginUser = async (
  loginData,
  useCookies = false,
  useSessionCookies = false
) => {
  const data = await apiRequest(
    `/login?useCookies=${useCookies}&useSessionCookies=${useSessionCookies}`,
    "POST",
    loginData
  );

  console.log("Login response:", data);

  if (useCookies) {
    console.log("Cookies are being used for authentication");
  } else {
    localStorage.setItem("accessToken", data.token);
    localStorage.setItem("refreshToken", data.refreshToken); 
  }

  return data;
};

// Logout a user
export const logoutUser = async (userId, useCookies = false) => {
  try {
    await apiRequest(`/logout?useCookies=${useCookies}`, "POST", userId, true);
   console.log("Logout response:", userId);
    console.log("Logout successful on backend");
  } catch (error) {
    console.error("Error during logout:", error);
  } finally {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    setTimeout(() => {
      window.location.href = "/login"; // Redirect to login page after logout
    }
    , 1000); // Adjust the delay as needed

    
  }
};

export const renewAuthToken = async ({ accessToken, refreshToken }) => {
  return await apiRequest(
    "/refresh-token",
    "POST",
    { accessToken, refreshToken },
    true
  );
};
