const API_URL = "/api/Contact/";

export const createContactUs = async (contactData) => {
  try {
    const response = await fetch(`${API_URL}/contact-us`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(contactData),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Failed to submit contact request.");
    }

    return await response.json();
  } catch (error) {
    console.error("Contact request error:", error);
    throw error;
  }
};