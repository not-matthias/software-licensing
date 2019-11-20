# software-licensing

## Motivation
Our target is to implement a software, which can send programs from the server to the client and execute it. The connection will be encrypted, to prevent any sniffing. Furthermore, the application will never be stored on the filesystem on the client, but immediately executed in memory.

## Realization

### Client

- Requests the program via a license_key
  - Get public key of the server
    ```
    GET {host}/license/public_key
    ```
  - Receive public key
    ```
    {
        "data": { /*public_key_data*/ },
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
  - Send request to the server  
    ```
    GET {host}/license/validate
    {
        "data": {
            "client_public_key":  "asdf",
            "license_key": "AAAA-BBBB-CCCC-DDDD",
        },
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the server.
  - Receive response with the program
    ```
    {
        "data": [4D, 5A, ...],
        "checksum": "813b91b01edabe5cc00cdd2f78ef9c22"
    }
    ```
    Note: The data is encrypted with the public key of the client.

## Problems

- MITM in the beginning: Attack is able to spoof the public_key, but he can't get the program.
- Server keys are generated when the program starts, instead of loading them from a file.
