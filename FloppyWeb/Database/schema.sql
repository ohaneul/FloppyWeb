-- Core tables for browser functionality

-- History table for visited pages
CREATE TABLE IF NOT EXISTS history (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    url TEXT NOT NULL,
    title TEXT,
    visit_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_visit TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    visit_count INTEGER DEFAULT 1,
    favicon_url TEXT,
    preview_image BLOB,
    UNIQUE(url)
);

-- Bookmarks with folder support
CREATE TABLE IF NOT EXISTS bookmarks (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    parent_id INTEGER,
    title TEXT NOT NULL,
    url TEXT,
    icon TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    sort_order INTEGER DEFAULT 0,
    is_folder BOOLEAN DEFAULT FALSE,
    FOREIGN KEY(parent_id) REFERENCES bookmarks(id) ON DELETE CASCADE
);

-- Downloads tracking
CREATE TABLE IF NOT EXISTS downloads (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    url TEXT NOT NULL,
    file_name TEXT NOT NULL,
    mime_type TEXT,
    size_bytes BIGINT,
    start_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    end_time TIMESTAMP,
    status TEXT CHECK(status IN ('in_progress', 'completed', 'failed', 'cancelled')),
    local_path TEXT,
    hash TEXT -- For integrity checking
);

-- Cache metadata
CREATE TABLE IF NOT EXISTS cache_entries (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    url TEXT NOT NULL,
    headers TEXT, -- JSON formatted headers
    content_type TEXT,
    size_bytes INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP,
    last_accessed TIMESTAMP,
    access_count INTEGER DEFAULT 1,
    UNIQUE(url)
);

-- User preferences
CREATE TABLE IF NOT EXISTS preferences (
    key TEXT PRIMARY KEY,
    value TEXT NOT NULL,
    data_type TEXT CHECK(data_type IN ('string', 'integer', 'boolean', 'json')),
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Search suggestions and autocomplete
CREATE TABLE IF NOT EXISTS search_suggestions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    query TEXT NOT NULL,
    result TEXT NOT NULL,
    frequency INTEGER DEFAULT 1,
    last_used TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(query, result)
);

-- Form autofill data (encrypted)
CREATE TABLE IF NOT EXISTS form_data (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    domain TEXT NOT NULL,
    field_name TEXT NOT NULL,
    encrypted_value BLOB NOT NULL,
    iv BLOB NOT NULL, -- Initialization vector for encryption
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_used TIMESTAMP,
    use_count INTEGER DEFAULT 1
);

-- Create indexes for performance
CREATE INDEX idx_history_url ON history(url);
CREATE INDEX idx_history_visit_time ON history(visit_time);
CREATE INDEX idx_bookmarks_parent ON bookmarks(parent_id);
CREATE INDEX idx_downloads_status ON downloads(status);
CREATE INDEX idx_cache_expires ON cache_entries(expires_at);
CREATE INDEX idx_form_data_domain ON form_data(domain);
CREATE INDEX idx_search_suggestions_query ON search_suggestions(query);

-- Views for common queries
CREATE VIEW v_recent_history AS
SELECT url, title, visit_time
FROM history
ORDER BY visit_time DESC
LIMIT 100;

CREATE VIEW v_popular_sites AS
SELECT url, title, visit_count
FROM history
ORDER BY visit_count DESC
LIMIT 20;

-- Triggers for maintenance
CREATE TRIGGER update_history_visit
AFTER INSERT ON history
BEGIN
    UPDATE history 
    SET visit_count = visit_count + 1,
        last_visit = CURRENT_TIMESTAMP
    WHERE url = NEW.url AND id != NEW.id;
END;

CREATE TRIGGER cleanup_old_history
AFTER INSERT ON history
BEGIN
    DELETE FROM history 
    WHERE visit_time < datetime('now', '-90 days')
    AND url NOT IN (SELECT url FROM bookmarks);
END;

CREATE TRIGGER update_bookmark_timestamp
AFTER UPDATE ON bookmarks
BEGIN
    UPDATE bookmarks 
    SET updated_at = CURRENT_TIMESTAMP 
    WHERE id = NEW.id;
END; 