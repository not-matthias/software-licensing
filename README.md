# software-licensing

## Motivation
Our target is to implement a software, which can send programs from the server to the client and execute it. The connection will be encrypted, to prevent any sniffing.

## Realization

### Client

- Requests the program via a license_key
  - Get public key of the server
    ```json
    GET {host}/public_key
    ```
  - Send request to the server  
    ```json
    GET {host}/program
    {
        "data": {
            "client_public_key":  "asdf",
            "license_key": "AAAA-BBBB-CCCC-DDDD",
        },
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the server.
  - Execute the program
    - Save it to file
    - Load it from memory
    ```json
    {
        "data": [],
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the client.


