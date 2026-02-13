-- ========================================
-- Database Setup Script for Product CRUD
-- PostgreSQL
-- ========================================

-- Create database (run this separately as postgres user)
-- CREATE DATABASE productdb;

-- Connect to the database
\c productdb;

-- ========================================
-- Create Products Table
-- ========================================
CREATE TABLE IF NOT EXISTS products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL CHECK (price >= 0),
    stock INTEGER NOT NULL DEFAULT 0 CHECK (stock >= 0),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- Create index on name for faster searches
CREATE INDEX IF NOT EXISTS idx_products_name ON products(name);

-- ========================================
-- Stored Procedure: Get All Products
-- ========================================
CREATE OR REPLACE FUNCTION sp_get_all_products()
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(200),
    description TEXT,
    price DECIMAL(10, 2),
    stock INTEGER,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.id,
        p.name,
        p.description,
        p.price,
        p.stock,
        p.created_at,
        p.updated_at
    FROM products p
    ORDER BY p.id DESC;
END;
$$;

-- ========================================
-- Stored Procedure: Get Product By ID
-- ========================================
CREATE OR REPLACE FUNCTION sp_get_product_by_id(p_id INTEGER)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(200),
    description TEXT,
    price DECIMAL(10, 2),
    stock INTEGER,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.id,
        p.name,
        p.description,
        p.price,
        p.stock,
        p.created_at,
        p.updated_at
    FROM products p
    WHERE p.id = p_id;
END;
$$;

-- ========================================
-- Stored Procedure: Create Product
-- ========================================
CREATE OR REPLACE FUNCTION sp_create_product(
    p_name VARCHAR(200),
    p_description TEXT,
    p_price DECIMAL(10, 2),
    p_stock INTEGER
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    new_id INTEGER;
BEGIN
    INSERT INTO products (name, description, price, stock, created_at)
    VALUES (p_name, p_description, p_price, p_stock, CURRENT_TIMESTAMP)
    RETURNING id INTO new_id;
    
    RETURN new_id;
END;
$$;

-- ========================================
-- Stored Procedure: Update Product
-- ========================================
CREATE OR REPLACE FUNCTION sp_update_product(
    p_id INTEGER,
    p_name VARCHAR(200),
    p_description TEXT,
    p_price DECIMAL(10, 2),
    p_stock INTEGER
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    rows_affected INTEGER;
BEGIN
    UPDATE products
    SET 
        name = p_name,
        description = p_description,
        price = p_price,
        stock = p_stock,
        updated_at = CURRENT_TIMESTAMP
    WHERE id = p_id;
    
    GET DIAGNOSTICS rows_affected = ROW_COUNT;
    RETURN rows_affected;
END;
$$;

-- ========================================
-- Stored Procedure: Delete Product
-- ========================================
CREATE OR REPLACE FUNCTION sp_delete_product(p_id INTEGER)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    rows_affected INTEGER;
BEGIN
    DELETE FROM products
    WHERE id = p_id;
    
    GET DIAGNOSTICS rows_affected = ROW_COUNT;
    RETURN rows_affected;
END;
$$;

-- ========================================
-- Insert Sample Data (Optional)
-- ========================================
INSERT INTO products (name, description, price, stock) VALUES
('Laptop', 'High-performance laptop for professionals', 1299.99, 15),
('Wireless Mouse', 'Ergonomic wireless mouse with USB receiver', 29.99, 50),
('USB-C Hub', '7-in-1 USB-C hub with HDMI and card reader', 49.99, 30),
('Mechanical Keyboard', 'RGB mechanical keyboard with blue switches', 89.99, 25),
('Monitor 27"', '4K UHD monitor with HDR support', 399.99, 10);

-- ========================================
-- Verify Setup
-- ========================================
SELECT 'Database setup completed successfully!' as message;
SELECT * FROM sp_get_all_products();
