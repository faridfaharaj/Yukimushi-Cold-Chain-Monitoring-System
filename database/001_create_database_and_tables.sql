CREATE TABLE dbo.kind (
    id   INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(16) NOT NULL UNIQUE
);

CREATE TABLE dbo.device_group (
    id     INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(40) NOT NULL
);

CREATE TABLE dbo.device (
    id       INT IDENTITY(1,1) PRIMARY KEY,
    group_id INT NOT NULL,
    kind_id  INT NOT NULL,
    mac      VARCHAR(40) NOT NULL UNIQUE,
    nombre   VARCHAR(40) NOT NULL,
    CONSTRAINT FK_device_group FOREIGN KEY (group_id) REFERENCES dbo.device_group(id),
    CONSTRAINT FK_device_kind  FOREIGN KEY (kind_id)  REFERENCES dbo.kind(id)
);

CREATE TABLE dbo.reading (
    id          INT IDENTITY(1,1) PRIMARY KEY,
    device_id   INT NOT NULL,
    kind_id     INT NOT NULL,
    recorded_at DATETIME2 NOT NULL,
    CONSTRAINT FK_reading_device FOREIGN KEY (device_id) REFERENCES dbo.device(id),
    CONSTRAINT FK_reading_kind   FOREIGN KEY (kind_id)   REFERENCES dbo.kind(id)
);

CREATE INDEX IX_reading_device_recorded_at
    ON dbo.reading (device_id, recorded_at);

CREATE TABLE dbo.geoespatial_reading (
    reading_id INT PRIMARY KEY,
    latitude   DECIMAL(9,6) NOT NULL,
    longitude  DECIMAL(9,6) NOT NULL,
    CONSTRAINT FK_geo_reading FOREIGN KEY (reading_id)
        REFERENCES dbo.reading(id) ON DELETE CASCADE
);

CREATE TABLE dbo.float_reading (
    reading_id INT PRIMARY KEY,
    value      DECIMAL(18,4) NOT NULL,
    CONSTRAINT FK_float_reading FOREIGN KEY (reading_id)
        REFERENCES dbo.reading(id) ON DELETE CASCADE
);

CREATE TABLE dbo.boolean_reading (
    reading_id INT PRIMARY KEY,
    value      BIT NOT NULL,
    CONSTRAINT FK_boolean_reading FOREIGN KEY (reading_id)
        REFERENCES dbo.reading(id) ON DELETE CASCADE
);
