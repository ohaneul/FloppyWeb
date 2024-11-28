use std::collections::HashMap;
use std::sync::{Arc, Mutex};
use chrono::{DateTime, Utc};
use serde::{Serialize, Deserialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct Cookie {
    name: String,
    value: String,
    domain: String,
    path: String,
    expires: Option<DateTime<Utc>>,
    secure: bool,
    http_only: bool,
    same_site: SameSitePolicy,
}

pub struct CookieManager {
    store: Arc<Mutex<HashMap<String, Vec<Cookie>>>>,
    encryption: Arc<CookieEncryption>,
}

impl CookieManager {
    pub fn new() -> Self {
        CookieManager {
            store: Arc::new(Mutex::new(HashMap::new())),
            encryption: Arc::new(CookieEncryption::new()),
        }
    }

    pub fn set_cookie(&self, domain: &str, cookie: Cookie) -> Result<(), CookieError> {
        let mut store = self.store.lock().unwrap();
        let domain_cookies = store.entry(domain.to_string())
            .or_insert_with(Vec::new);

        // Remove old cookie if it exists
        domain_cookies.retain(|c| c.name != cookie.name);
        
        // Add new cookie
        if cookie.secure {
            let encrypted = self.encryption.encrypt_cookie(&cookie)?;
            domain_cookies.push(encrypted);
        } else {
            domain_cookies.push(cookie);
        }

        Ok(())
    }

    pub fn get_cookies(&self, domain: &str) -> Vec<Cookie> {
        let store = self.store.lock().unwrap();
        let now = Utc::now();

        store.get(domain)
            .map(|cookies| {
                cookies.iter()
                    .filter(|c| c.expires.map_or(true, |exp| exp > now))
                    .cloned()
                    .collect()
            })
            .unwrap_or_default()
    }
} 